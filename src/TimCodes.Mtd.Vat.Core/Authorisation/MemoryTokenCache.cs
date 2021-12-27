using Microsoft.Extensions.Caching.Memory;

namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public class MemoryTokenCache : ITokenCache
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "Mtd.Vat.AccessToken";
        public MemoryTokenCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task StoreTokenAsync(AccessToken token)
        {
            _cache.Set(CacheKey, token, token.Expiry);
            return Task.CompletedTask;
        }

        public Task<bool> TryGetAsync(out AccessToken? token)
        {
            return Task.FromResult(_cache.TryGetValue(CacheKey, out token));
        }

        public Task ClearAsync()
        {
            _cache.Remove(CacheKey);
            return Task.CompletedTask;
        }
    }
}
