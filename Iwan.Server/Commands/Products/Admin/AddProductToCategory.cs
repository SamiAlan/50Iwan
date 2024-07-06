using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{
    public class AddProductToCategory
    {
        public record Request(string ProductId, string CategoryId) : IRequest<ProductCategoryDto>;

        public class Handler : IRequestHandler<Request, ProductCategoryDto>
        {
            protected readonly IProductService _productService;
            protected readonly IQueryProductService _queryProductService;

            public Handler(IProductService productService, IQueryProductService queryProductService)
            {
                _productService = productService;
                _queryProductService = queryProductService;
            }

            public async Task<ProductCategoryDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var productCategory = await _productService.AddProductToCategoryAsync(request.ProductId, request.CategoryId, cancellationToken);

                return await _queryProductService.GetProductCategoryDetailsAsync(productCategory.Id, cancellationToken);
            }
        }
    }
}
