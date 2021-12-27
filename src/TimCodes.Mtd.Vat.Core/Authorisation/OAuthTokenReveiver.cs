using Microsoft.Extensions.Options;
using System.Text.Json;
using TimCodes.Mtd.Vat.Core.Configuration;
using TimCodes.Mtd.Vat.Core.Constants;

namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public class OAuthTokenReveiver : ITokenReceiver
    {
        private readonly ITokenCache _cache;
        private readonly HttpClient _client;
        private readonly MtdOptions _options;

        public OAuthTokenReveiver(ITokenCache cache, HttpClient client, IOptions<MtdOptions> options)
        {
            _cache = cache;
            _client = client;
            _options = options.Value;
            _client.BaseAddress = _options.TokenUri;
        }

        public async Task<AccessToken?> GetAccessTokenAsync(string code)
        {
            var form = new Dictionary<string, string>
                {
                    {"grant_type", "authorization_code"},
                    {"client_id", _options.ClientId},
                    {"client_secret", _options.ClientSecret},
                    {"redirect_uri", AuthorisationConstants.RedirectUri},
                    {"code", code}
                };

            var tokenResponse = await _client.PostAsync(string.Empty, new FormUrlEncodedContent(form));
            return await ReadTokenResponseAsync(tokenResponse).ConfigureAwait(false);
        }

        public async Task<AccessToken?> RefreshAccessTokenAsync(string refreshToken)
        {
            var form = new Dictionary<string, string>
                {
                    {"grant_type", "refresh_token"},
                    {"client_id", _options.ClientId},
                    {"client_secret", _options.ClientSecret},
                    {"refresh_token", refreshToken}
                };

            var tokenResponse = await _client.PostAsync(string.Empty, new FormUrlEncodedContent(form));
            return await ReadTokenResponseAsync(tokenResponse).ConfigureAwait(false);
        }

        private async Task<AccessToken?> ReadTokenResponseAsync(HttpResponseMessage message)
        {
            var jsonContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            var token = JsonSerializer.Deserialize<AccessToken>(jsonContent);
            if (token is not null)
            {
                token.Expiry = DateTime.Now.AddSeconds(token.ExpiresIn);
            }
            return token;
        }
    }
}
