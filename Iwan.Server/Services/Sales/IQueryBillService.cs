using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Options.Sales;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sales
{
    public interface IQueryBillService
    {
        Task<BillDto> GetBillDetailsAsync(string billId, CancellationToken cancellationToken = default);
        Task<BillItemDto> GetBillItemDetailsAsync(string billItemId, CancellationToken cancellationToken = default);
        Task<PagedDto<BillDto>> GetBillsAsync(GetBillsOptions options, CancellationToken cancellationToken);
    }
}
