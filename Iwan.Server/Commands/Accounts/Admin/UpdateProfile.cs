using Iwan.Server.Constants;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Accounts.Admin
{
    public class UpdateProfile
    {
        public record Request(UpdateProfileDto Profile) : IRequest<UserDto>;

        public class Handler : IRequestHandler<Request, UserDto>
        {
            protected readonly IAccountService _accountService;
            protected readonly IQueryAccountService _queryAccountService;
            protected readonly ILoggedInUserProvider _loggedInUserProvider;

            public Handler(IAccountService accountService,
                IQueryAccountService queryAccountService,
                ILoggedInUserProvider loggedInUserProvider)
            {
                _accountService = accountService;
                _queryAccountService = queryAccountService;
                _loggedInUserProvider = loggedInUserProvider;
            }


            public async Task<UserDto> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    await _accountService.UpdateProfileAsync(request.Profile, cancellationToken);

                    return await _queryAccountService.GetUserDetailsAsync(_loggedInUserProvider.UserId, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch
                {
                    throw new ServerErrorException(Responses.Accounts.ServerErrorWhenUpdatingUserProfile);
                }
            }
        }
    }
}
