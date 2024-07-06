using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{
    public class EditProduct
    {
        public record Request(EditProductDto EditedProduct) : IRequest<ProductDto>;

        public class Handler : IRequestHandler<Request, ProductDto>
        {
            protected readonly IProductService _productService;
            protected readonly IQueryProductService _queryProductService;

            public Handler(IProductService productService, IQueryProductService queryProductService)
            {
                _productService = productService;
                _queryProductService = queryProductService;
            }

            public async Task<ProductDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var product = await _productService.EditProductAsync(request.EditedProduct, cancellationToken);

                return await _queryProductService.GetProductDetailsAsync(product.Id, false, false, cancellationToken);
            }
        }
    }
}
