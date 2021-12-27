using System.Text.Json.Serialization;

namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public class AccessToken
    {
        public AccessToken(string token, string refresh, DateTime expiry)
        {
            Token = token;
            Refresh = refresh;
            Expiry = expiry;
        }

        [JsonPropertyName("access_token")]
        public string Token { get; init; }

        [JsonPropertyName("refresh_token")]
        public string Refresh { get; init; }

        public DateTime Expiry { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

    }
}
