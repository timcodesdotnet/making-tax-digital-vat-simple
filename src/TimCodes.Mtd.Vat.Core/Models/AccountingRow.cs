using TimCodes.Mtd.Vat.Core.Models.Requests;

namespace TimCodes.Mtd.Vat.Core.Models
{
    public class AccountingRow
    {
        public int RowNumber { get; set; }
        public string? InvoiceDate { get; set; }
        public string? DatePaid { get; set; }
        public string? Company { get; set; }
        public string? Description { get; set; }
        public string? AmountPaidExVat { get; set; }
        public string? AmountSoldExVat { get; set; }
        public string? VatToReclaim { get; set; }
        public string? VatDue { get; set; }
        public string? VatRate { get; set; }
        public string? TotalExpenses { get; set; }
        public string? TotalRevenue { get; set; }
    }

    public class ValidatedVatRow
    {
        public string? Company { get; set; }
        public decimal AmountPaidExVat { get; set; }
        public decimal AmountSoldExVat { get; set; }
        public decimal VatToReclaim { get; set; }
        public decimal VatDue { get; set; }

        public static VatReturnRequest? Validate(AccountingRow row)
        {
            if (row == null) return null;
            var request = new VatReturnRequest();

            if (!decimal.TryParse(row.AmountPaidExVat, out var num)) return null;
            request.TotalValuePurchasesExVAT = (int)Math.Round(-num);
            if (!decimal.TryParse(row.AmountSoldExVat, out num)) return null;
            request.TotalValueSalesExVAT = (int)Math.Round(num);
            if (!decimal.TryParse(row.VatToReclaim, out num)) return null;
            request.VatReclaimedCurrPeriod = num;
            if (!decimal.TryParse(row.VatDue, out num)) return null;
            request.VatDueSales = num;

            return request;
        }
    }
}
