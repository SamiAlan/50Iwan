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
    public class AddProduct
    {
        public record Request(AddProductDto Product) : IRequest<ProductDto>;

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
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var product = await _productService.AddProductAsync(request.Product, cancellationToken);

                        var productDto = await _queryProductService.GetProductDetailsAsync(product.Id, true, true, cancellationToken);

                        scope.Complete();

                        return productDto;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Products.ErrorAddingProduct); }
                }
            }
        }
    }
}
