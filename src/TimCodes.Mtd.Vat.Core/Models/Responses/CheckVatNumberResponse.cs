namespace TimCodes.Mtd.Vat.Core.Models.Responses
{
    public class CheckVatNumberResponse : Response
    {
        public CheckVatNumberTarget? Target { get; set; }

        public DateTimeOffset? ProcessingDate { get; set; }
    }

    public class CheckVatNumberTarget
    {
        public string? Name { get; set; }

        public string? VatNumber { get; set; }

        public Address? Address { get; set; }
    }
}
