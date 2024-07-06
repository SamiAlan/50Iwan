using Iwan.Server.Services.Products;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Iwan.Server.Commands.Products.Admin
{
    public class DeleteState
    {
        public record Request(string StateId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IProductService _productService;

            public Handler(IProductService productService)
            {
                _productService = productService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _productService.DeleteProductStateAsync(request.StateId, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
