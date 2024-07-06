using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Accounts.Admin
{
    public class GetCurrentUser
    {
        public record Request : IRequest<UserDto>;

        public class Handler : IRequestHandler<Request, UserDto>
        {
            protected readonly IQueryAccountService _queryAccountService;
            protected readonly ILoggedInUserProvider _loggedInUserProvider;

            public Handler(IQueryAccountService queryAccountService,
                ILoggedInUserProvider loggedInUserProvider)
            {
                _queryAccountService = queryAccountService;
                _loggedInUserProvider = loggedInUserProvider;
            }

            public async Task<UserDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryAccountService.GetUserDetailsAsync(_loggedInUserProvider.UserId, cancellationToken);
            }
        }
    }
}
