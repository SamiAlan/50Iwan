using Iwan.Server.Services.Vendors;
using Iwan.Shared.Dtos.Vendors;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Vendors.Admin
{
    public class EditVendor
    {
        public record Request(EditVendorDto Vendor) : IRequest<VendorDto>;

        public class Handler : IRequestHandler<Request, VendorDto>
        {
            protected readonly IVendorService _vendorService;
            protected readonly IQueryVendorService _queryVendorService;

            public Handler(IVendorService vendorService, IQueryVendorService queryVendorService)
            {
                _vendorService = vendorService;
                _queryVendorService = queryVendorService;
            }

            public async Task<VendorDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var vendor = await _vendorService.EditVendorAsync(request.Vendor, cancellationToken);

                return await _queryVendorService.GetVendorDetailsAsync(vendor.Id, true, cancellationToken);
            }
        }
    }
}
