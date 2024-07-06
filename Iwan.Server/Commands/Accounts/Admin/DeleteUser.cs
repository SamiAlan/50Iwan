using Iwan.Server.Constants;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Accounts.Admin
{
    public class DeleteUser
    {
        public record Request(string Id) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IAccountService _accountService;

            public Handler(IAccountService accountService)
            {
                _accountService = accountService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    await _accountService.DeleteUserAsync(request.Id, cancellationToken);

                    return Unit.Value;
                }
                catch (BaseException) { throw; }
                catch
                {
                    throw new ServerErrorException(Responses.Accounts.ServerErrorWhenDeletingUser);
                }
            }
        }
    }
}
