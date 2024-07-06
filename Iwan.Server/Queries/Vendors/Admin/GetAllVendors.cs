using Iwan.Server.Services.Vendors;
using Iwan.Shared.Dtos.Vendors;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Vendors.Admin
{
    public class GetAllVendors
    {
        public record Request : IRequest<IEnumerable<VendorDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<VendorDto>>
        {
            protected readonly IQueryVendorService _queryVendorService;

            public Handler(IQueryVendorService queryVendorService)
            {
                _queryVendorService = queryVendorService;
            }

            public async Task<IEnumerable<VendorDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryVendorService.GetAllVendorsAsync(false, cancellationToken);
            }
        }
    }
}
