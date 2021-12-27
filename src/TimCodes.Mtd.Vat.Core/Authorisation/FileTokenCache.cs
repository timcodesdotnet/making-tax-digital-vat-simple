using System.Text.Json;

namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public class FileTokenCache : ITokenCache
    {
        private readonly string _path;

        public FileTokenCache()
        {
            _path = Path.Combine(Directory.GetCurrentDirectory(), "auth.json");
        }

        public async Task StoreTokenAsync(AccessToken token)
        {
            await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(token));
        }

        public Task<bool> TryGetAsync(out AccessToken? token)
        {
            var text = File.Exists(_path) ? File.ReadAllText(_path) : null;
            if (string.IsNullOrEmpty(text))
            {
                token = null;
                return Task.FromResult(false);
            }

            token = JsonSerializer.Deserialize<AccessToken>(text);
            return Task.FromResult(true);
        }

        public Task ClearAsync()
        {
            File.Delete(_path);
            return Task.CompletedTask;
        }
    }
}
