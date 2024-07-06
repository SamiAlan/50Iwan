using Iwan.Server.Services.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Common.Admin
{
    public class DeleteAddress
    {
        public record Request(string AddressId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IAddressService _addressService;

            public Handler(IAddressService addressService)
            {
                _addressService = addressService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _addressService.DeleteAddressAsync(request.AddressId, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
