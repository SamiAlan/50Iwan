using Iwan.Server.Models.Pages;
using Iwan.Server.Options;
using Iwan.Server.Services.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Pages
{
    public class GetProductsPageContent
    {
        public record Request(GetProductsPageOptions Options) : IRequest<ProductsPageContentViewModel>;

        public class Handler : IRequestHandler<Request, ProductsPageContentViewModel>
        {
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesQueryService pagesQueryService)
            {
                _pagesQueryService = pagesQueryService;
            }

            public async Task<ProductsPageContentViewModel> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _pagesQueryService.GetProductsPageContentAsync(request.Options, cancellationToken);
            }
        }
    }

    public class GetSearchPageContent
    {
        public record Request(GetSearchProductsOptions Options) : IRequest<SearchPageContentViewModel>;

        public class Handler : IRequestHandler<Request, SearchPageContentViewModel>
        {
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesQueryService pagesQueryService)
            {
                _pagesQueryService = pagesQueryService;
            }

            public async Task<SearchPageContentViewModel> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _pagesQueryService.GetSearchPageContentAsync(request.Options, cancellationToken);
            }
        }
    }
}
