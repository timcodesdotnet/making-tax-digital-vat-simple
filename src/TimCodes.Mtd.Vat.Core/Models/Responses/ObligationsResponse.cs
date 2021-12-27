namespace TimCodes.Mtd.Vat.Core.Models.Responses
{
    public class ObligationsResponse : Response
    {
        public Obligation[] Obligations { get; set; }
    }

    public class Obligation
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Due { get; set; }

        public DateTime? Received { get; set; }

        public string Status { get; set; }

        public string PeriodKey { get; set; }
    }
}
