using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Products.Admin
{
    public class GetProduct
    {
        public record Request(string ProductId) : IRequest<ProductDto>;

        public class Handler : IRequestHandler<Request, ProductDto>
        {
            protected readonly IQueryProductService _queryProductService;

            public Handler(IQueryProductService queryProductService)
            {
                _queryProductService = queryProductService;
            }

            public async Task<ProductDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryProductService.GetProductDetailsAsync(request.ProductId, true, true, cancellationToken);
            }
        }
    }
}
