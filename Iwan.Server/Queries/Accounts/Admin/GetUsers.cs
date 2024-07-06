using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Options.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Accounts.Admin
{
    public class GetUsers
    {
        public record Request(GetUsersOptions Options) :  IRequest<PagedDto<UserDto>>;

        public class Handler : IRequestHandler<Request, PagedDto<UserDto>>
        {
            protected readonly IQueryAccountService _queryAccountService;

            public Handler(IQueryAccountService queryAccountService)
            {
                _queryAccountService = queryAccountService;
            }

            public async Task<PagedDto<UserDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryAccountService.GetUsersDetailsAsync(request.Options, cancellationToken);
            }
        }
    }
}
