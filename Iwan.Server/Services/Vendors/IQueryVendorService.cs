using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Options.Vendors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Vendors
{
    public interface IQueryVendorService
    {
        Task<IEnumerable<VendorDto>> GetAllVendorsAsync(bool includeAddresses = false, CancellationToken cancellationToken = default);
        Task<PagedDto<VendorDto>> GetVendorsAsync(GetVendorsOptions options, CancellationToken cancellationToken = default);
        Task<VendorDto> GetVendorDetailsAsync(string vendorId, bool includeAddress = true, CancellationToken cancellationToken = default);
    }
}
