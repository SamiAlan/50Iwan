using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{
    public class AddState
    {
        public record Request(AddProductStateDto State) : IRequest<ProductStateDto>;

        public class Handler : IRequestHandler<Request, ProductStateDto>
        {
            protected readonly IProductService _productService;
            protected readonly IQueryProductService _queryProductService;

            public Handler(IProductService productService, IQueryProductService queryProductService)
            {
                _productService = productService;
                _queryProductService = queryProductService;
            }

            public async Task<ProductStateDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var state = await _productService.AddProductStateAsync(request.State, cancellationToken);

                var productStateDto = await _queryProductService.GetProductStateDetailsAsync(state.Id, cancellationToken);

                return productStateDto;
            }
        }
    }
}
