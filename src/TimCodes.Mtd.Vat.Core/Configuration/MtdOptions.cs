namespace TimCodes.Mtd.Vat.Core.Configuration
{
#pragma warning disable CS8618
    public class MtdOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public Uri ApiBaseUri { get; set; }

        public Uri AuthUri { get; set; }

        public Uri TokenUri { get; set; }

        public string Scope { get; set; }

        public string VatRegistrationNumber { get; set; }

        public string Version { get; set; }

        public string ProductName { get; set; }

        public string MfaKey { get; set; }
    }
#pragma warning restore CS8618
}
