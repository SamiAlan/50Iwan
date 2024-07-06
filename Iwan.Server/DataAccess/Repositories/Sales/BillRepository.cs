using Iwan.Server.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Sales
{
    public class BillRepository : Repository<Bill>, IBillRepository
    {
        public BillRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Bill>> GetAllBillsWithAllInfoAsync(CancellationToken cancellationToken = default)
        {
            return await Table.Include(b => b.BillItems).ThenInclude(i => i.Product)
                .ThenInclude(p => p.Vendor).ToListAsync(cancellationToken);
        }

        public async Task<Bill> GetAsync(string billId, bool includeBillItems = false, bool includeBillItemsProducts =
            false, CancellationToken cancellationToken = default)
        {
            var query = Table;

            query = (includeBillItems, includeBillItemsProducts) switch
            {
                (true, true) => query.Include(b => b.BillItems).ThenInclude(i => i.Product),
                (true, false) => query.Include(b => b.BillItems),
                (false, false) => query,
                _ => throw new Exception("Unsupported situation"),
            };

            return await query.SingleOrDefaultAsync(b => b.Id == billId, cancellationToken);
        }
    }

}
