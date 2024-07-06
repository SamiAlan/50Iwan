using Iwan.Server.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Sales
{
    public class BillItemRepository : Repository<BillItem>, IBillItemRepository
    {
        public BillItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<BillItem> GetAsync(string billItemId, bool includeBill = false, bool includeProduct = false, CancellationToken
            cancellationToken = default)
        {
            var query = Table;

            if (includeBill)
                query = query.Include(i => i.Bill);

            if (includeProduct)
                query = query.Include(i => i.Product);

            return await query.SingleOrDefaultAsync(i => i.Id == billItemId, cancellationToken);
        }
    }
}
