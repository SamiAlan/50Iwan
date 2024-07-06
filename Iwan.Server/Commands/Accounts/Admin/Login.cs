using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Accounts.Admin
{
    /// <summary>
    /// Represents a login command containing the request and the handler types
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Represents the request type for the login command
        /// </summary>
        public record Request(LoginDto LoginCredentials) : IRequest<IAuthToken>;

        /// <summary>
        /// Represents the handler for the <see cref="Request"/> class
        /// </summary>
        public class Handler : IRequestHandler<Request, IAuthToken>
        {
            protected readonly IAccountService _accountService;

            /// <summary>
            /// Contructs a new intance of the <see cref="Handler"/> class
            /// </summary>
            public Handler(IAccountService accountService)
            {
                _accountService = accountService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            /// <param name="request">The request object</param>
            public async Task<IAuthToken> Handle(Request request, CancellationToken cancellationToken = default)
            {
                return await _accountService.LoginAsync(request.LoginCredentials, cancellationToken);
            }
        }
    }
}
