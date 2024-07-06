using Iwan.Server.Services.Vendors;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Options.Vendors;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Vendors.Admin
{
    public class GetVendors
    {
        public record Request(GetVendorsOptions Options) : IRequest<PagedDto<VendorDto>>;

        public class Handler : IRequestHandler<Request, PagedDto<VendorDto>>
        {
            protected readonly IQueryVendorService _queryVendorService;

            public Handler(IQueryVendorService queryVendorService)
            {
                _queryVendorService = queryVendorService;
            }

            public async Task<PagedDto<VendorDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryVendorService.GetVendorsAsync(request.Options, cancellationToken);
            }
        }
    }
}
