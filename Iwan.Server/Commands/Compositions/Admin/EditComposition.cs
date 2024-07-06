using Iwan.Server.Infrastructure.DataAccess;
using Iwan.Server.Services.Compositions;
using Iwan.Shared.Dtos.Compositions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Compositions.Admin
{
    public class EditComposition
    {
        public record Request(EditCompositionDto Composition) : IRequest<CompositionDto>;

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
                var composition = await _compositionService.EditCompositionAsync(request.Composition, cancellationToken);

                return await _queryCompositionService.GetCompositionDetailsAsync(composition.Id, false, cancellationToken);

            }
        }
    }
}
