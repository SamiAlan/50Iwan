using Iwan.Server.Services.Compositions;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Options.Compositions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Compositions
{
    public class GetCompositions
    {
        public record Request(GetCompositionsOptions Options) : IRequest<PagedDto<CompositionDto>>;

        public class Handler : IRequestHandler<Request, PagedDto<CompositionDto>>
        {
            protected readonly IQueryCompositionService _queryCompositionService;

            public Handler(IQueryCompositionService queryCompositionService)
            {
                _queryCompositionService = queryCompositionService;
            }

            public async Task<PagedDto<CompositionDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryCompositionService.GetCompositionsAsync(request.Options, cancellationToken);
            }
        }
    }
}
