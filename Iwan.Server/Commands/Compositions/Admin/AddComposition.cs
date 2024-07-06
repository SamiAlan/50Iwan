using Iwan.Server.Constants;
using Iwan.Server.Services.Compositions;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Compositions.Admin
{
    public class AddComposition
    {
        public record Request(AddCompositionDto Composition) : IRequest<CompositionDto>;

        public class Handler : IRequestHandler<Request, CompositionDto>
        {
            protected readonly ICompositionService _compositionService;
            protected readonly IQueryCompositionService _queryCompositionService;

            public Handler(ICompositionService compositionService,
                IQueryCompositionService queryCompositionService)
            {
                _compositionService = compositionService;
                _queryCompositionService = queryCompositionService;
            }

            public async Task<CompositionDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var composition = await _compositionService.AddCompositionAsync(request.Composition,
                            cancellationToken);

                        var compositionDto = await _queryCompositionService.GetCompositionDetailsAsync(composition.Id, true, cancellationToken);

                        scope.Complete();

                        return compositionDto;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Compositions.ErrorAddingComposition);  }
                }
            }
        }
    }
}
