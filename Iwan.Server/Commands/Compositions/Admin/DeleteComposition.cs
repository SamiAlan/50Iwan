using Iwan.Server.Constants;
using Iwan.Server.Infrastructure.DataAccess;
using Iwan.Server.Services.Compositions;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Compositions.Admin
{
    public class DeleteComposition
    {
        public record Request(string CompositionId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly ICompositionService _compositionService;

            public Handler(ICompositionService compositionService)
            {
                _compositionService = compositionService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _compositionService.DeleteCompositionAsync(request.CompositionId, cancellationToken);

                        scope.Complete();

                        return Unit.Value;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Compositions.ErrorDeletingComposition); }
                }
            }
        }
    }
}
