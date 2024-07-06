using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
namespace Iwan.Server.Commands.Products.Admin
{
    public class ChangeProductMainImage
    {
        public record Request(ChangeProductMainImageDto Image) : IRequest<ProductMainImageDto>;

        public class Handler : IRequestHandler<Request, ProductMainImageDto>
        {
            protected readonly IProductService _productService;
            protected readonly IQueryProductService _queryProductService;

            public Handler(IProductService productService, IQueryProductService queryProductService)
            {
                _productService = productService;
                _queryProductService = queryProductService;
            }

            public async Task<ProductMainImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var productImage = await _productService.ChangeProductMainImageAsync(request.Image, cancellationToken);

                return await _queryProductService.GetProductMainImageDetailsAsync(productImage.Id, cancellationToken);
            }
        }
    }
}
