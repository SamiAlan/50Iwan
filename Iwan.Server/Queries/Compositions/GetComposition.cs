using Iwan.Server.Services.Compositions;
using Iwan.Shared.Dtos.Compositions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Compositions
{
    public class GetComposition
    {
        public record Request(string CompositionId) : IRequest<CompositionDto>;

        public class Handler : IRequestHandler<Request, CompositionDto>
        {
            protected readonly IQueryCompositionService _queryCompositionService;

            public Handler(IQueryCompositionService queryCompositionService)
            {
                _queryCompositionService = queryCompositionService;
            }

            public async Task<CompositionDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryCompositionService.GetCompositionDetailsAsync(request.CompositionId, true, cancellationToken);
            }
        }
    }
}
