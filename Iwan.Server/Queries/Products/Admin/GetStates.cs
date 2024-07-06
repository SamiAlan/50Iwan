using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Products.Admin
{
    public class GetStates
    {
        public record Request(string ProductId) : IRequest<IEnumerable<ProductStateDto>>;

        public class Handler : IRequestHandler<Request, IEnumerable<ProductStateDto>>
        {
            protected readonly IQueryProductService _queryProductService;

            public Handler(IQueryProductService queryProductService)
            {
                _queryProductService = queryProductService;
            }

            public async Task<IEnumerable<ProductStateDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryProductService.GetProductStatesAsync(request.ProductId, cancellationToken);
            }
        }
    }
}
