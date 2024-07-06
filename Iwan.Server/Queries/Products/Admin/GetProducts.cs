using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Options.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Products.Admin
{
    public class GetProducts
    {
        public record Request(AdminGetProductsOptions Options) : IRequest<PagedDto<ProductDto>>;

        public class Handler : IRequestHandler<Request, PagedDto<ProductDto>>
        {
            protected readonly IQueryProductService _queryProductService;

            public Handler(IQueryProductService queryProductService)
            {
                _queryProductService = queryProductService;
            }

            public async Task<PagedDto<ProductDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _queryProductService.GetProductsAsync(request.Options, cancellationToken);
            }
        }
    }
}
