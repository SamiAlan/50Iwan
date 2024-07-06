using Iwan.Shared.Constants;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Iwan.Shared.Exceptions;
using Iwan.Server.Constants;

namespace Iwan.Server.Commands.Accounts.Admin
{
    /// <summary>
    /// Represents an add user (for admin users) command containing the request and the handler types
    /// </summary>
    public class AddUser
    {
        /// <summary>
        /// Represents the request type for the add user command
        /// </summary>
        public record Request(AddUserDto User) : IRequest<UserDto>;

        /// <summary>
        /// Represents the handler for the <see cref="Request"/> class
        /// </summary>
        public class Handler : IRequestHandler<Request, UserDto>
        {
            protected readonly IAccountService _accountsService;
            protected readonly IQueryAccountService _queryAccountsService;

            public Handler(IAccountService accountsService, IQueryAccountService queryAccountsService)
            {
                _accountsService = accountsService;
                _queryAccountsService = queryAccountsService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            /// <param name="request">The request object</param>
            public async Task<UserDto> Handle(Request request, CancellationToken cancellationToken = default)
            {
                try
                {
                    var addedUser = await _accountsService.AddUserAsync(request.User, Roles.Admin, cancellationToken);

                    return await _queryAccountsService.GetUserDetailsAsync(addedUser.Id, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch
                {
                    throw new ServerErrorException(Responses.Accounts.ServerErrorWhenAddingUser);
                }
            }
        }
    }
}