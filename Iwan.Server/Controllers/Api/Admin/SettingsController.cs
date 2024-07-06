using Iwan.Server.Commands.Settings.Admin;
using Iwan.Server.Events.Settings;
using Iwan.Server.Queries.Settings.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class SettingsController : BaseAdminApiController
    {
        public SettingsController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer)
            : base(mediator, stringLocalizer) { }

        [HttpGet]
        [Route(Routes.Api.Admin.Settings.Images)]
        public async Task<IActionResult> GetImagesSettings(CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new GetImagesSettings.Request(), cancellationToken);

            return Ok(settings);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Settings.TEMPIMAGES_BASE)]
        public async Task<IActionResult> GetTempImagesSettings(CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new GetTempImagesSettings.Request(), cancellationToken);

            return Ok(settings);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Settings.WATERMARK_BASE)]
        public async Task<IActionResult> GetWatermarkSettings(CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new GetWatermarkSettings.Request(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.Images_Categories)]
        public async Task<IActionResult> EditCategoriesImagesSettings(CategoriesImagesSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveCategoriesImagesSettings.Request(request), cancellationToken);

            await _mediator.Publish(new CategoriesImagesSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.Images_AboutUsSection)]
        public async Task<IActionResult> EditAboutUsImagesSettings(AboutUsSectionImagesSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveAboutUsSectionImagesSettings.Request(request), cancellationToken);

            await _mediator.Publish(new AboutUsSectionImagesSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.Images_Products)]
        public async Task<IActionResult> EditProductsImagesSettings(ProductsImagesSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveProductsImagesSettings.Request(request), cancellationToken);

            await _mediator.Publish(new ProductsImagesSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.Images_Compositions)]
        public async Task<IActionResult> EditCompositionsImagesSettings(CompositionsImagesSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveCompositionsImagesSettings.Request(request), cancellationToken);

            await _mediator.Publish(new CompositionsImagesSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.Images_Slider_Images)]
        public async Task<IActionResult> EditSlidersImagesSettings(SlidersImagesSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveSlidersImagesSettings.Request(request), cancellationToken);

            await _mediator.Publish(new SlidersImagesSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.TEMPIMAGES_BASE)]
        public async Task<IActionResult> EditTempImagesSettings(TempImagesSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveTempImagesBackgroundServiceSettings.Request(request), cancellationToken);

            await _mediator.Publish(new TempImagesSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.WATERMARK_BASE)]
        public async Task<IActionResult> EditWatermarkSettings(EditWatermarkSettingsDto request, CancellationToken cancellationToken = default)
        {
            var settings = await _mediator.Send(new SaveWatermarkSettings.Request(request), cancellationToken);

            await _mediator.Publish(new WatermarkSettingsEditedEvent(), cancellationToken);

            return Ok(settings);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Settings.ChangeWatermarkImage)]
        public async Task<IActionResult> ChangeWatermarkImage(ChangeWatermarkImageDto request, CancellationToken cancellationToken = default)
        {
            var newImage = await _mediator.Send(new ChangeWatermarkImage.Request(request), cancellationToken);

            await _mediator.Publish(new WatermarkImageChangedEvent(), cancellationToken);

            return Ok(newImage);
        }
    }
}
