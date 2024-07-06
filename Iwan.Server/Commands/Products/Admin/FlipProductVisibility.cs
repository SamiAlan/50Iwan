using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{
    public class FlipProductVisibility
    {
        public record Request(string Id) : IRequest<Unit>;
        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.ProductsRepository.FindAsync(request.Id, cancellationToken);

                if (product == null)
                    throw new NotFoundException(Responses.Products.ProductNotFound);

                product.IsVisible = !product.IsVisible;

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
