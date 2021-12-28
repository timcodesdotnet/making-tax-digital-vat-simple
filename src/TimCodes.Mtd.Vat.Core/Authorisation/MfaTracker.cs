namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public static class MfaTracker
    {
        public static DateTime LastChecked { get; set; }

        public static string? Identifier { get; set; }
    }
}
