using TimCodes.Mtd.Vat.Core.Models.Requests;
using TimCodes.Mtd.Vat.Core.Models.Responses;

namespace TimCodes.Mtd.Vat.Core.Services
{
    public interface ISubmissionHistoryService
    {
        Task AuditAsync(VatReturnRequest request, VatReturnResponse response);
    }
}
