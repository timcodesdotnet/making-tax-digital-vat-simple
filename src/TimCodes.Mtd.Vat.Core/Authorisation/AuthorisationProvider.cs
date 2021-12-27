using Microsoft.Extensions.Logging;

namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public class AuthorisationProvider
    {
        private readonly ITokenCache _cache;
        private readonly ITokenReceiver _tokenReceiver;
        private readonly ILogger<AuthorisationProvider> _logger;

        public AuthorisationProvider(ITokenCache cache, ITokenReceiver tokenReceiver, ILogger<AuthorisationProvider> logger)
        {
            _cache = cache;
            _tokenReceiver = tokenReceiver;
            _logger = logger;
        }

        public async Task<AccessToken?> GetAccessTokenAsync(string code)
        {
            _logger.LogInformation("Requesting new access token with auth code");
            var token = await _tokenReceiver.GetAccessTokenAsync(code).ConfigureAwait(false);
            if (token is not null)
            {
                await _cache.StoreTokenAsync(token).ConfigureAwait(false);
                _logger.LogInformation("Cached new access token");
            }
            return token;
        }


        public async Task<AccessToken?> GetAccessTokenAsync()
        {
            if (await _cache.TryGetAsync(out var accessToken))
            {
                if (accessToken == null)
                {
                    _logger.LogInformation("No access token cached");
                    return null;
                }

                _logger.LogInformation("Found cached access token");
                if (accessToken.Expiry < DateTime.Now)
                {
                    _logger.LogInformation("Cached access token is expired, requesting a new one with refresh token");
                    accessToken = await _tokenReceiver.RefreshAccessTokenAsync(accessToken.Refresh).ConfigureAwait(false);
                    if (accessToken is not null)
                    {
                        await _cache.StoreTokenAsync(accessToken).ConfigureAwait(false);
                        _logger.LogInformation("Cached new access token");
                    }
                    else
                    {
                        await _cache.ClearAsync().ConfigureAwait(false);
                        _logger.LogInformation("Refresh token failed, clearing cache");
                    }
                }
                return accessToken;
            }
            _logger.LogInformation("No access token cached");
            return null;
        }

        public async Task ClearCacheAsync()
        {
            await _cache.ClearAsync();
        }
    }
}
