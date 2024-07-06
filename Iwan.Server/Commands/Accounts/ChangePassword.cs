using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Accounts
{
    /// <summary>
    /// Represents a change password command containing the request and the handler types
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        /// Represents the request type for the change password command
        /// </summary>
        public record Request(ChangePasswordDto ChangePassword) : IRequest<Unit>;

        /// <summary>
        /// Represents the handler for the <see cref="Request"/> class
        /// </summary>
        public class Handler : IRequestHandler<Request, Unit>
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
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken = default)
            {
                await _accountService.ChangePasswordAsync(request.ChangePassword, cancellationToken);

                return Unit.Value;
            }
        }
    }
}