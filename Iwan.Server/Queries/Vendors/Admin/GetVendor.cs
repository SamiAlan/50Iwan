using Iwan.Server.Services.Vendors;
using Iwan.Shared.Dtos.Vendors;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Vendors.Admin
{
    public class GetVendor
    {
        public record Request(string VendorId) : IRequest<VendorDto>;

        public class Handler : IRequestHandler<Request, VendorDto>
        {
            protected readonly IQueryVendorService _queryVendorService;

            public Handler(IQueryVendorService queryVendorService)
            {
                _queryVendorService = queryVendorService;
            }

            public async Task<VendorDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryVendorService.GetVendorDetailsAsync(request.VendorId, true, cancellationToken);
            }
        }
    }
}
