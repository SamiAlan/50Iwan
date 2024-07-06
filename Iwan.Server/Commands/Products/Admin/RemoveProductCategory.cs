using Iwan.Server.Services.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{
    public class RemoveProductCategory
    {
        public record Request(string ProductCategoryId) : IRequest<(string, string)>;

        public class Handler : IRequestHandler<Request, (string, string)>
        {
            protected readonly IProductService _productService;

            public Handler(IProductService productService)
            {
                _productService = productService;
            }

            public async Task<(string, string)> Handle(Request request, CancellationToken cancellationToken)
            {
                var ids = await _productService.RemoveProductCategoryAsync(request.ProductCategoryId, cancellationToken);

                return ids;
            }
        }
    }
}
