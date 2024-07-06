using Iwan.Shared.Exceptions;
using Iwan.Server.Services.Media;
using Iwan.Server.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Media.Admin
{
    public class DeleteTempImage
    {
        public record Request(string TempImageId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IImageService _imagesService;

            public Handler(IImageService imagesService)
            {
                _imagesService = imagesService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _imagesService.DeleteTempImageAsync(request.TempImageId, true, true, cancellationToken);

                        scope.Complete();

                        return Unit.Value;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Images.ErrorWhileDeletingImage); }
                }
            }
        }
    }
}
