using Iwan.Server.Domain.Vendors;
using Iwan.Shared.Dtos.Vendors;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Vendors
{
    public interface IVendorService
    {
        Task<Vendor> AddVendorAsync(AddVendorDto vendorToAdd, CancellationToken cancellationToken = default);
        Task<Vendor> EditVendorAsync(EditVendorDto editedVendor, CancellationToken cancellationToken = default);
        Task DeleteVendorAsync(string vendorId, CancellationToken cancellationToken = default);
    }
}
