using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Web;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Configuration;
using TimCodes.Mtd.Vat.Core.Constants;
using TimCodes.Mtd.Vat.Core.Models.Requests;
using TimCodes.Mtd.Vat.Core.Models.Responses;

namespace TimCodes.Mtd.Vat.Core.Services
{
    public class VatService : IVatService
    {
        private const string BusinessNameCacheKey = "Mtd.Vat.BusinessName";

        private readonly MtdOptions _options;
        private readonly IMemoryCache _memoryCache;
        private readonly AuthorisationProvider _authorisationProvider;
        private readonly HttpClient _client;
        private readonly ISubmissionHistoryService _submissionHistoryService;
        private readonly IUserIdService _userIdService;
        private readonly ILogger<VatService> _logger;

        public VatService(
            IOptions<MtdOptions> options, 
            IMemoryCache memoryCache, 
            AuthorisationProvider authorisationProvider, 
            HttpClient client,
            ISubmissionHistoryService submissionHistoryService,
            IUserIdService userIdService,
            ILogger<VatService> logger)
        {
            _options = options.Value;
            _memoryCache = memoryCache;
            _authorisationProvider = authorisationProvider;
            _client = client;
            _submissionHistoryService = submissionHistoryService;
            _userIdService = userIdService;
            _logger = logger;
            _client.BaseAddress = _options.ApiBaseUri;
            _client.DefaultRequestHeaders.Add("Accept", HeaderConstants.AcceptHmrcJson);
        }

        public async Task<string> GetBusinessName()
        {
            if (!_memoryCache.TryGetValue(BusinessNameCacheKey, out CheckVatNumberResponse? value))
            {
                value = await CheckVatNumberAsync().ConfigureAwait(false);
                _memoryCache.Set(BusinessNameCacheKey, value);
            }

            return value?.Target?.Name ?? "Unknown";
        }

        public async Task<ObligationsResponse?> GetObligationsAsync(DateTime from, DateTime to, FraudPreventionData fraudPreventionData)
        {
            var token = await _authorisationProvider.GetAccessTokenAsync();
            if (token != null)
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"organisations/vat/{_options.VatRegistrationNumber}/obligations?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                requestMessage.Headers.Add("Accept", HeaderConstants.AcceptHmrcJson);
                AddFraudPreventionHeaders(requestMessage, fraudPreventionData);
                var httpResponse = await _client.SendAsync(requestMessage).ConfigureAwait(false);
                return await Read<ObligationsResponse>(httpResponse).ConfigureAwait(false);
            }
            _logger.LogError("Access token not available to get obligations");
            return null;
        }

        public async Task<VatReturnResponse?> SubmitVatReturnAsync(VatReturnRequest request, FraudPreventionData fraudPreventionData)
        {
            var token = await _authorisationProvider.GetAccessTokenAsync();
            if (token != null)
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"organisations/vat/{_options.VatRegistrationNumber}/returns");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                requestMessage.Headers.Add("Accept", HeaderConstants.AcceptHmrcJson);

                AddFraudPreventionHeaders(requestMessage, fraudPreventionData);
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }), Encoding.UTF8, HeaderConstants.AcceptJson);

                var httpResponse = await _client.SendAsync(requestMessage).ConfigureAwait(false);
                var response = await Read<VatReturnResponse>(httpResponse).ConfigureAwait(false);
                if (response != null) 
                {
                    if (response.WasSuccessful)
                    {
                        httpResponse.Headers.TryGetValues("Receipt-ID", out var values);
                        response.ReceiptId = values?.FirstOrDefault();
                        await _submissionHistoryService.AuditAsync(request, response).ConfigureAwait(false);
                    }
                }
                return response;
            }
            _logger.LogError("Access token not available to submit vat return");
            return null;
        }

        private async Task<CheckVatNumberResponse?> CheckVatNumberAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"organisations/vat/check-vat-number/lookup/{_options.VatRegistrationNumber}");
            var httpResponse = await _client.SendAsync(requestMessage).ConfigureAwait(false);

            return await Read<CheckVatNumberResponse>(httpResponse).ConfigureAwait(false);
        }

        private async Task<T?> Read<T>(HttpResponseMessage message)
            where T : Response
        {
            var content = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonSerializer.Deserialize<T?>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response is null)
            {
                _logger.LogError("Response was null");
                return response;
            }

            _logger.LogInformation($"Response received Success: {message.IsSuccessStatusCode}, Status Code: {message.StatusCode}, Message: {content}");

            response.WasSuccessful = message.IsSuccessStatusCode;
            return response;
        }

        private void AddFraudPreventionHeaders(HttpRequestMessage message, FraudPreventionData fraudPreventionData)
        {
            message.Headers.Add("Gov-Client-Connection-Method", "DESKTOP_APP_DIRECT");
            message.Headers.Add("Gov-Client-Device-ID", _userIdService.GetDeviceId());
            message.Headers.Add("Gov-Client-Local-IPs", string.Join(",", GetIps()));
            message.Headers.Add("Gov-Client-Local-IPs-Timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.fffZ"));
            message.Headers.Add("Gov-Client-MAC-Addresses", string.Join(",", GetMacAddress()));
            message.Headers.Add("Gov-Client-Multi-Factor", $"type=TOTP&timestamp={HttpUtility.UrlEncode(MfaTracker.LastChecked.ToString("yyyy-MM-ddThh:mmZ"))}&unique-reference={HttpUtility.UrlEncode(MfaTracker.Identifier)}");
            message.Headers.Add("Gov-Client-Screens", fraudPreventionData.Screens);
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            message.Headers.Add("Gov-Client-Timezone", $"UTC{(offset.TotalMinutes < 0 ? "-" : "+")}{offset:hh\\:mm}");
            message.Headers.Add("Gov-Client-User-Agent", $"os-family={HttpUtility.UrlEncode(Environment.OSVersion.Platform.ToString())}&os-version={HttpUtility.UrlEncode(Environment.OSVersion.VersionString)}&device-manufacturer=PC%20Specialist&device-model=Custom");
            message.Headers.Add("Gov-Client-Window-Size", fraudPreventionData.Window);
            message.Headers.Add("Gov-Client-User-IDs", $"os={Environment.UserName}");
            message.Headers.Add("Gov-Vendor-License-IDs", $"{_options.ProductName}=7abc1a233092fc104c7af72a89c0829c");
            message.Headers.Add("Gov-Vendor-Product-Name", _options.ProductName);
            message.Headers.Add("Gov-Vendor-Version", $"{_options.ProductName}={_options.Version}");
        }

        private static List<string> GetIps()
        {
            var hostName = Dns.GetHostName();

            var iphostentry = Dns.GetHostEntry(hostName);

            return iphostentry.AddressList.Select(q => HttpUtility.UrlEncode(q.ToString())).ToList();
        }

        private static List<string> GetMacAddress()
        {
            return
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select HttpUtility.UrlEncode(nic.GetPhysicalAddress().ToString())
                ).Where(q=>!string.IsNullOrEmpty(q)).ToList();
        }
    }
}
