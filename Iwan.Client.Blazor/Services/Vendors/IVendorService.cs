using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Options.Vendors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Vendors
{
    public interface IVendorService
    {
        Task<PagedDto<VendorDto>> GetVendorsAsync(GetVendorsOptions options, CancellationToken cancellationToken = default);
        Task<VendorDto> GetVendorAsync(string vendorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<VendorDto>> GetAllVendorsAsync(CancellationToken cancellationToken = default);
        Task<VendorDto> AddVendorAsync(AddVendorDto vendorToAdd, CancellationToken cancellationToken = default);
        Task<VendorDto> EditVendorAsync(EditVendorDto editedVendor, CancellationToken cancellationToken = default);
        Task DeleteVendorAsync(string vendorId, CancellationToken cancellationToken = default);
    }
}
