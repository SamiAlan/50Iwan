using Iwan.Server.Domain.Sales;
using System.Threading;
using System.Threading.Tasks;
using Iwan.Shared.Dtos.Sales;

namespace Iwan.Server.Services.Sales
{
    public interface IBillService
    {
        Task<Bill> AddBillAsync(AddBillDto billToAdd, CancellationToken cancellationToken = default);

        Task<BillItem> AddBillItemAsync(string billId, AddBillItemDto itemToAdd, CancellationToken cancellationToken = 
            default);

        Task RemoveBillItemAsync(string billItemId, CancellationToken cancellationToken = default);

        Task DeleteBillAsync(string billId, CancellationToken cancellationToken = default);

        Task<Bill> EditBillAsync(EditBillDto editedBill, CancellationToken cancellationToken = default);

        Task<BillItem> EditBillItemAsync(EditBillItemDto editedBillItem, CancellationToken cancellationToken = default);
    }
}
