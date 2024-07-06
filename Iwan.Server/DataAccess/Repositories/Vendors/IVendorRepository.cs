using Iwan.Server.Domain.Vendors;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Vendors
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<Vendor> GetAsync(string vendorId, bool includeAddress, CancellationToken cancellationToken = default);
    }
}
