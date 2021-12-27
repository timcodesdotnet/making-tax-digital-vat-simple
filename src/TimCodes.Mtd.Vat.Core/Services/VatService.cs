using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Configuration;
using TimCodes.Mtd.Vat.Core.Constants;
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
        private readonly ILogger<VatService> _logger;

        public VatService(
            IOptions<MtdOptions> options, 
            IMemoryCache memoryCache, 
            AuthorisationProvider authorisationProvider, 
            HttpClient client,
            ILogger<VatService> logger)
        {
            _options = options.Value;
            _memoryCache = memoryCache;
            _authorisationProvider = authorisationProvider;
            _client = client;
            _logger = logger;
            _client.BaseAddress = _options.ApiBaseUri;
            _client.DefaultRequestHeaders.Add("Accept", HeaderConstants.Accept);
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

        public async Task<ObligationsResponse?> GetObligationsAsync(DateTime from, DateTime to)
        {
            var token = await _authorisationProvider.GetAccessTokenAsync();
            if (token != null)
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"organisations/vat/{_options.VatRegistrationNumber}/obligations?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
                requestMessage.Headers.Add("Accept", HeaderConstants.Accept);

                var httpResponse = await _client.SendAsync(requestMessage).ConfigureAwait(false);
                return await Read<ObligationsResponse>(httpResponse).ConfigureAwait(false);
            }
            _logger.LogError("Access token not available to get obligations");
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

            _logger.LogInformation($"Response received Success: {message.IsSuccessStatusCode}, Status Code: {message.StatusCode}, Message: {response.Message}");

            response.WasSuccessful = message.IsSuccessStatusCode;
            return response;
        }
    }
}
