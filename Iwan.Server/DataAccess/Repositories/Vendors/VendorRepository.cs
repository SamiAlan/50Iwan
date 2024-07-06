using Iwan.Server.Domain.Vendors;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Vendors
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Vendor> GetAsync(string vendorId, bool includeAddress = true, CancellationToken cancellationToken = default)
        {
            if (!includeAddress)
                return await FindAsync(vendorId, cancellationToken);

            return await Table.Include(v => v.Address).SingleOrDefaultAsync(v => v.Id == vendorId, cancellationToken);
        }
    }
}
