namespace TimCodes.Mtd.Vat.Core.Models.Requests
{
    public class VatReturnRequest
    {
        /// <summary>
        /// The ID code for the period that this obligation belongs to. The format is a string of four alphanumeric characters. Occasionally the format includes the # symbol.
        /// </summary>
        public string? PeriodKey { get; set; }

        /// <summary>
        /// VAT due on sales and other outputs. 
        /// This corresponds to box 1 on the VAT Return form
        /// </summary>
        public decimal VatDueSales { get; set; }

        /// <summary>
        /// VAT due in the period on acquisitions of goods made in Northern Ireland from EU Member States. 
        /// This corresponds to box 2 on the VAT Return form
        /// </summary>
        public decimal VatDueAcquisitions { get; set; }

        /// <summary>
        /// Total VAT due (the sum of vatDueSales and vatDueAcquisitions). 
        /// This corresponds to box 3 on the VAT Return form
        /// </summary>
        public decimal TotalVatDue => VatDueSales + VatDueAcquisitions;

        /// <summary>
        /// VAT reclaimed in the period on purchases and other inputs (including acquisitions in Northern Ireland from EU member states). 
        /// This corresponds to box 4 on the VAT Return form
        /// </summary>
        public decimal VatReclaimedCurrPeriod { get; set; }

        /// <summary>
        /// The difference between totalVatDue and vatReclaimedCurrPeriod. 
        /// This corresponds to box 5 on the VAT Return form
        /// </summary>
        public decimal NetVatDue => Math.Abs(TotalVatDue - VatReclaimedCurrPeriod);

        /// <summary>
        /// Total value of sales and all other outputs excluding any VAT. 
        /// This corresponds to box 6 on the VAT Return form
        /// </summary>
        public int TotalValueSalesExVAT { get; set; }

        /// <summary>
        /// Total value of purchases and all other inputs excluding any VAT (including exempt purchases). 
        /// This corresponds to box 7 on the VAT Return form
        /// </summary>
        public int TotalValuePurchasesExVAT { get; set; }

        /// <summary>
        /// Total value of dispatches of goods and related costs (excluding VAT) from Northern Ireland to EU Member States. 
        /// This corresponds to box 8 on the VAT Return form
        /// </summary>
        public int TotalValueGoodsSuppliedExVAT { get; set; }

        /// <summary>
        /// Total value of acquisitions of goods and related costs (excluding VAT) made in Northern Ireland from EU Member States. 
        /// This corresponds to box 9 on the VAT Return form
        /// </summary>
        public int TotalAcquisitionsExVAT { get; set; }

        /// <summary>
        /// Declaration that the user has finalised their VAT return.
        /// </summary>
        public bool Finalised { get; set; }
    }
}
