using Iwan.Server.Domain.Sales;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Sales
{
    public interface IBillItemRepository : IRepository<BillItem>
    {
        Task<BillItem> GetAsync(string billItemId, bool includeBill = false, bool includeProduct = false,
            CancellationToken cancellationToken = default);
    }
}
