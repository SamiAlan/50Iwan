using Iwan.Server.Services.Common;
using Iwan.Shared.Dtos.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Common.Admin
{
    public class GetAddress
    {
        public record Request(string AddressId) : IRequest<AddressDto>;

        public class Handler : IRequestHandler<Request, AddressDto>
        {
            protected readonly IQueryAddressService _queryAddressService;

            public Handler(IQueryAddressService queryAddressService)
            {
                _queryAddressService = queryAddressService;
            }

            public async Task<AddressDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryAddressService.GetAddressDetailsAsync(request.AddressId, cancellationToken);
            }
        }
    }
}
