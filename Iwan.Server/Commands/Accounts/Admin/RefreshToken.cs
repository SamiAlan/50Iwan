using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Accounts.Admin
{
    /// <summary>
    /// Represents a refresh token command containing the request and the handler types
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Represents the request type for the refresh token command
        /// </summary>
        public record Request(RefreshUserTokenDto RefreshUserToken) : IRequest<IAuthToken>;

        /// <summary>
        /// Represents the handler for the <see cref="Request"/> class
        /// </summary>
        public class Handler : IRequestHandler<Request, IAuthToken>
        {
            protected readonly IAccountService _accountService;

            public Handler(IAccountService accountService)
            {
                _accountService = accountService;
            }

            /// <summary>
            /// Handles when a <see cref="Request"/> is sent
            /// </summary>
            /// <param name="request">The request object</param>
            public async Task<IAuthToken> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _accountService.RefreshTokenAsync(request.RefreshUserToken, cancellationToken);
            }
        }
    }
}
