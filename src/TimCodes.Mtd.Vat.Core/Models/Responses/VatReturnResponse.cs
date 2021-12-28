namespace TimCodes.Mtd.Vat.Core.Models.Responses
{
    public class VatReturnResponse : Response
    {
        public DateTime ProcessingDate { get; set; }

        public string? PaymentIndicator { get; set; }

        public string? FormBundleNumber { get; set; }

        public string? ChargeRefNumber { get; set; }
    }
}
