using System.Text;
using TimCodes.Mtd.Vat.Core.Models.Requests;
using TimCodes.Mtd.Vat.Core.Models.Responses;
using TimCodes.Mtd.Vat.Core.Services;

namespace TimCodes.Mtd.Vat.Core.Authorisation
{
    public class FileSubmissionHistoryService : ISubmissionHistoryService
    {
        private const char Comma = ',';

        private readonly string _path;

        public FileSubmissionHistoryService()
        {
            _path = Path.Combine(Directory.GetCurrentDirectory(), "submissions.csv");
        }

        public async Task AuditAsync(VatReturnRequest request, VatReturnResponse response)
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.Append(request.PeriodKey);
            sb.Append(Comma);
            sb.Append(response.ReceiptId);
            sb.Append(Comma);
            sb.Append(response.FormBundleNumber);
            sb.Append(Comma);
            sb.Append(response.ProcessingDate);
            sb.Append(Comma);
            sb.Append(request.VatDueSales);
            sb.Append(Comma);
            sb.Append(request.VatDueAcquisitions);
            sb.Append(Comma);
            sb.Append(request.TotalVatDue);
            sb.Append(Comma);
            sb.Append(request.VatReclaimedCurrPeriod);
            sb.Append(Comma);
            sb.Append(request.NetVatDue);
            sb.Append(Comma);
            sb.Append(request.TotalValueSalesExVAT);
            sb.Append(Comma);
            sb.Append(request.TotalValuePurchasesExVAT);
            sb.Append(Comma);
            sb.Append(request.TotalValueGoodsSuppliedExVAT);
            sb.Append(Comma);
            sb.Append(request.TotalAcquisitionsExVAT);

            if (!File.Exists(_path))
            {
                await File.AppendAllTextAsync(_path, "Period,Receipt,Bundle Number,Date,Box 1,Box 2,Box 3,Box 4,Box 5,Box 6,Box 7,Box 8,Box 9");
            }
            await File.AppendAllTextAsync(_path, sb.ToString());
        }
    }
}
