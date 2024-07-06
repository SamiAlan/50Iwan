using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Pages.Admin
{
    public class GetProductDetailsPageContent
    {
        public record Request : IRequest<ProductDetailsPageContentDto>;

        public class Handler : IRequestHandler<Request, ProductDetailsPageContentDto>
        {
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesQueryService homeQueryService)
            {
                _pagesQueryService = homeQueryService;
            }

            public async Task<ProductDetailsPageContentDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _pagesQueryService.GetProductDetailsContentPageAsync(cancellationToken);
            }
        }
    }
}
