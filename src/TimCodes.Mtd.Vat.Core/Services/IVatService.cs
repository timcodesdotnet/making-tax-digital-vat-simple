using TimCodes.Mtd.Vat.Core.Models.Requests;
using TimCodes.Mtd.Vat.Core.Models.Responses;

namespace TimCodes.Mtd.Vat.Core.Services
{
    public interface IVatService
    {
        Task<string> GetBusinessName();
        Task<ObligationsResponse?> GetObligationsAsync(DateTime from, DateTime to);
        Task<VatReturnResponse?> SubmitVatReturnAsync(VatReturnRequest request);
    }
}
