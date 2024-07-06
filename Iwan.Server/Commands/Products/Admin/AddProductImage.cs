using Iwan.Server.Constants;
using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Products.Admin
{
    public class AddProductImage
    {
        public record Request(AddProductImageDto Image) : IRequest<ProductImageDto>;

        public class Handler : IRequestHandler<Request, ProductImageDto>
        {
            protected readonly IProductService _productService;
            protected readonly IQueryProductService _queryProductService;

            public Handler(IProductService productService, IQueryProductService queryProductService)
            {
                _productService = productService;
                _queryProductService = queryProductService;
            }

            public async Task<ProductImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var productImage = await _productService.AddProductImageAsync(request.Image.ProductId, request.Image, cancellationToken);

                        var productImageDto = await _queryProductService.GetProductImageDetailsAsync(productImage.Id, true, cancellationToken);

                        scope.Complete();

                        return productImageDto;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Products.ErrorAddingProduct); }
                }

            }
        }
    }
}
