using Iwan.Server.Services.Sliders;
using Iwan.Shared.Dtos.Sliders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Sliders.Admin
{
    public class EditSliderImage
    {
        public record Request(EditSliderImageDto EditedImage) : IRequest<SliderImageDto>;

        public class Handler : IRequestHandler<Request, SliderImageDto>
        {
            protected readonly ISliderService _sliderImage;
            protected readonly IQuerySliderService _querySliderImage;

            public Handler(ISliderService sliderImage, IQuerySliderService querySliderImage)
            {
                _sliderImage = sliderImage;
                _querySliderImage = querySliderImage;
            }

            public async Task<SliderImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var sliderImage = await _sliderImage.EditSliderImageAsync(request.EditedImage, cancellationToken);

                return await _querySliderImage.GetSliderImageDetailsAsync(sliderImage.Id, cancellationToken);
            }
        }
    }
}
