using Iwan.Server.Constants;
using Iwan.Server.Services.Vendors;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Vendors.Admin
{
    public class DeleteVendor
    {
        public record Request(string VendorId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IVendorService _vendorService;

            public Handler(IVendorService vendorService)
            {
                _vendorService = vendorService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    await _vendorService.DeleteVendorAsync(request.VendorId, cancellationToken);

                    return Unit.Value;
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Vendors.ErrorAddingVendor); }
            }
        }
    }
}
