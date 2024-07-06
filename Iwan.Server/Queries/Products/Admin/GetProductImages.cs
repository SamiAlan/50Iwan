using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Products.Admin
{
    public class GetProductImages
    {
        public record Request(string Id) : IRequest<IEnumerable<ProductImageDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<ProductImageDto>>
        {
            protected readonly IQueryProductService _queryProductService;

            public Handler(IQueryProductService queryProductService)
            {
                _queryProductService = queryProductService;
            }

            public async Task<IEnumerable<ProductImageDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryProductService.GetProductImagesDetailsAsync(request.Id, cancellationToken);
            }
        }
    }
}
