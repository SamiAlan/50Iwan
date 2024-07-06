using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Options.Products;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Products.Admin
{
    public class GetAllProducts
    {
        public record Request(GetAllProductsOptions Options) : IRequest<IEnumerable<ProductDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<ProductDto>>
        {
            protected readonly IQueryProductService _queryProductService;

            public Handler(IQueryProductService queryProductService)
            {
                _queryProductService = queryProductService;
            }

            public async Task<IEnumerable<ProductDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryProductService.GetProductsAsync(request.Options, cancellationToken);
            }
        }
    }
}
