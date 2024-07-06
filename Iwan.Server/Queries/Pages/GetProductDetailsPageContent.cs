using Iwan.Server.Models.Pages;
using Iwan.Server.Services.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Pages
{
    public class GetProductDetailsPageContent
    {
        public record Request(string ProductId, string CategoryId) : IRequest<ProductDetailsPageContentViewModel>;

        public class Handler : IRequestHandler<Request, ProductDetailsPageContentViewModel>
        {
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesQueryService pagesQueryService)
            {
                _pagesQueryService = pagesQueryService;
            }

            public async Task<ProductDetailsPageContentViewModel> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _pagesQueryService.GetProductDetailsPageContentAsync(request.ProductId, request.CategoryId, cancellationToken);
            }
        }
    }
}
