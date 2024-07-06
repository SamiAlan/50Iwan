using Iwan.Server.Constants;
using Iwan.Server.Services.Vendors;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Vendors.Admin
{
    public class AddVendor
    {
        public record Request(AddVendorDto Vendor) : IRequest<VendorDto>;

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
                try
                {
                    var vendor = await _vendorService.AddVendorAsync(request.Vendor, cancellationToken);

                    return await _queryVendorService.GetVendorDetailsAsync(vendor.Id, true, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Vendors.ErrorAddingVendor); }
            }
        }
    }
}
