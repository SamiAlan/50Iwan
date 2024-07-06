using Iwan.Server.Constants;
using Iwan.Server.Services.Sliders;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Sliders.Admin
{
    public class DeleteSliderImage
    {
        public record Request(string SliderImageId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly ISliderService _sliderService;

            public Handler(ISliderService sliderService)
            {
                _sliderService = sliderService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _sliderService.DeleteSliderImageAsync(request.SliderImageId, true, cancellationToken);
                        
                        scope.Complete();
                        
                        return Unit.Value; 
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Sliders.ErrorDeletingSliderImage); }
                }
            }
        }
    }
}
