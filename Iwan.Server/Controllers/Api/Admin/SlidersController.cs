using Iwan.Server.Commands.Sliders.Admin;
using Iwan.Server.Events.Sliders;
using Iwan.Server.Queries.Sliders.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Options.SliderImages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class SlidersController : BaseAdminApiController
    {
        public SlidersController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer)
            : base(mediator, stringLocalizer) { }

        [HttpGet]
        [Route(Routes.Api.Admin.Sliders.BASE)]
        public async Task<IActionResult> Get([FromQuery] GetSliderImagesOptions options, CancellationToken cancellationToken = default)
        {
            var images = await _mediator.Send(new GetSliderImages.Request(options), cancellationToken);

            return Ok(images);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Sliders.GetSlider)]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            var image = await _mediator.Send(new GetSliderImage.Request(id), cancellationToken);

            return Ok(image);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Sliders.BASE)]
        public async Task<IActionResult> Post(AddSliderImageDto request, CancellationToken cancellationToken = default)
        {
            var sliderImage = await _mediator.Send(new AddSliderImage.Request(request), cancellationToken);

            await _mediator.Publish(new SliderImageAddedEvent(sliderImage.Id), cancellationToken);

            return Ok(sliderImage);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Sliders.BASE)]
        public async Task<IActionResult> Edit(EditSliderImageDto request, CancellationToken cancellationToken = default)
        {
            var sliderImage = await _mediator.Send(new EditSliderImage.Request(request), cancellationToken);

            await _mediator.Publish(new SliderImageEditedEvent(sliderImage.Id), cancellationToken);

            return Ok(sliderImage);
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Sliders.Delete)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteSliderImage.Request(id), cancellationToken);

            await _mediator.Publish(new SliderImageDeletedEvent(id), cancellationToken);

            return Ok();
        }
    }
}
