using Iwan.Server.Constants;
using Iwan.Server.Services.Sliders;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Iwan.Server.Commands.Sliders.Admin
{
    public class AddSliderImage
    {
        public record Request(AddSliderImageDto Image) : IRequest<SliderImageDto>;

        public class Handler : IRequestHandler<Request, SliderImageDto>
        {
            protected readonly ISliderService _sliderService;
            protected readonly IQuerySliderService _querySliderService;

            public Handler(ISliderService sliderService, IQuerySliderService querySliderService)
            {
                _sliderService = sliderService;
                _querySliderService = querySliderService;
            }

            public async Task<SliderImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var sliderImage = await _sliderService.AddSliderImageAsync(request.Image, cancellationToken);

                        var sliderImageDto = await _querySliderService.GetSliderImageDetailsAsync(sliderImage.Id, cancellationToken);

                        scope.Complete();

                        // Return the new slider image
                        return sliderImageDto;
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Sliders.ErrorAddingImage); }
                }
            }
        }
    }
}
