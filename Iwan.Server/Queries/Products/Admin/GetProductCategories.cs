using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Products.Admin
{
    public class GetProductCategories
    {
        public record Request(string ProductId) : IRequest<IEnumerable<ProductCategoryDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<ProductCategoryDto>>
        {
            protected readonly IQueryProductService _queryProductService;

            public Handler(IQueryProductService queryProductService)
            {
                _queryProductService = queryProductService;
            }

            public async Task<IEnumerable<ProductCategoryDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryProductService.GetProductCategoriesAsync(productId: request.ProductId, cancellationToken);
            }
        }
    }
}
