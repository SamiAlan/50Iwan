using Iwan.Server.Domain.Sales;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Sales
{
    public interface IBillRepository : IRepository<Bill>
    {
        Task<Bill> GetAsync(string billId, bool includeBillItems = false, bool includeBillItemsProducts =
            false, CancellationToken cancellationToken = default);

        Task<IEnumerable<Bill>> GetAllBillsWithAllInfoAsync(CancellationToken cancellationToken = default);
    }
}
