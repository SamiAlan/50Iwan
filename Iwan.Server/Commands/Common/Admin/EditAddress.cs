using Iwan.Server.Services.Common;
using Iwan.Shared.Dtos.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Common.Admin
{
    public class EditAddress
    {
        public record Request(EditAddressDto Address) : IRequest<AddressDto>;

        public class Handler : IRequestHandler<Request, AddressDto>
        {
            protected readonly IAddressService _addressService;
            protected readonly IQueryAddressService _queryAddressService;

            public Handler(IAddressService addressService, IQueryAddressService queryAddressService)
            {
                _queryAddressService = queryAddressService;
                _addressService = addressService;
            }

            public async Task<AddressDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var address = await _addressService.EditAddressAsync(request.Address, cancellationToken);

                return await _queryAddressService.GetAddressDetailsAsync(address.Id, cancellationToken);
            }
        }
    }
}
