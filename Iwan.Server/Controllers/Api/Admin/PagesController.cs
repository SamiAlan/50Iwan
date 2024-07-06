using Iwan.Server.Commands.Pages.Admin;
using Iwan.Server.Events.Pages;
using Iwan.Server.Queries.Pages.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class PagesController : BaseAdminApiController
    {
        public PagesController(IMediator mediator, IStringLocalizer<Localization> localizer) : base(mediator, localizer)
        {
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Pages.BASE_HOME)]
        public async Task<IActionResult> GetHomePageContent(CancellationToken cancellationToken = default)
        {
            var content = await _mediator.Send(new GetHomePageContent.Request(), cancellationToken);

            return Ok(content);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Pages.BASE_PRODUCT_DETAILS)]
        public async Task<IActionResult> GetProductDetailsPageContent(CancellationToken cancellationToken = default)
        {
            var content = await _mediator.Send(new GetProductDetailsPageContent.Request(), cancellationToken);

            return Ok(content);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Pages.BASE_COLORS)]
        public async Task<IActionResult> AddColor(AddColorDto color, CancellationToken cancellationToken = default)
        {
            var addedColor = await _mediator.Send(new AddColor.Request(color), cancellationToken);

            await _mediator.Publish(new ColorAddedEvent(addedColor.Id), cancellationToken);

            return Ok(addedColor);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Pages.BASE_HEADER)]
        public async Task<IActionResult> EditHeaderSection(EditHeaderSectionDto header, CancellationToken cancellationToken = default)
        {
            var section = await _mediator.Send(new EditHeaderSection.Request(header), cancellationToken);

            await _mediator.Publish(new HeaderSectionEditedEvent(), cancellationToken);

            return Ok(section);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Pages.BASE_CONTACTUS)]
        public async Task<IActionResult> EditContactUsSection(EditContactUsSectionDto contactUs, CancellationToken cancellationToken = default)
        {
            var section = await _mediator.Send(new EditContactUsSection.Request(contactUs), cancellationToken);

            await _mediator.Publish(new ContactUsSectionEditedEvent(), cancellationToken);

            return Ok(section);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Pages.BASE_SERVICES)]
        public async Task<IActionResult> EditServicesSection(EditServicesSectionDto services, CancellationToken cancellationToken = default)
        {
            var section = await _mediator.Send(new EditServicesSection.Request(services), cancellationToken);

            await _mediator.Publish(new ServicesSectionEditedEvent(), cancellationToken);

            return Ok(section);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Pages.BASE_ABOUTUS)]
        public async Task<IActionResult> EditAboutUsSection(EditAboutUsSectionDto aboutUs, CancellationToken cancellationToken = default)
        {
            var section = await _mediator.Send(new EditAboutUsSection.Request(aboutUs), cancellationToken);

            await _mediator.Publish(new AboutUsSectionEditedEvent(), cancellationToken);

            return Ok(section);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Pages.BASE_ABOUTUS_IMAGES)]
        public async Task<IActionResult> AddAboutUsSectionImage(AddAboutUsSectionImageDto imageToAdd, CancellationToken cancellationToken = default)
        {
            var addedImage = await _mediator.Send(new AddAboutUsSectionImage.Request(imageToAdd), cancellationToken);

            await _mediator.Publish(new AboutUsSectionImageAddedEvent(addedImage.Id), cancellationToken);

            return Ok(addedImage);
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Pages.DeleteAboutUsImage)]
        public async Task<IActionResult> DeleteAboutUsImage(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteAboutUsImage.Request(id), cancellationToken);

            await _mediator.Publish(new AboutUsImageDeletedEvent(id), cancellationToken);

            return Ok();
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Pages.DeleteColor)]
        public async Task<IActionResult> DeleteColor(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteColor.Request(id), cancellationToken);

            await _mediator.Publish(new ColorDeletedEvent(id), cancellationToken);

            return Ok();
        }


        [HttpPut]
        [Route(Routes.Api.Admin.Pages.BASE_INTERIOR_DESIGN)]
        public async Task<IActionResult> EditInteriorDesignSection(EditInteriorDesignSectionDto interiorDesign, CancellationToken cancellationToken = default)
        {
            var section = await _mediator.Send(new EditInteriorDesignSection.Request(interiorDesign), cancellationToken);

            await _mediator.Publish(new InteriorDesignSectionEditedEvent(), cancellationToken);

            return Ok(section);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Pages.ChangeInteriorDesignMainImage)]
        public async Task<IActionResult> ChangeInteriorDesignImage(ChangeInteriorDesignSectionMainImageDto newImage, CancellationToken cancellationToken = default)
        {
            var newAddedImage = await _mediator.Send(new ChangeInteriorDesignSectionImage.Request(newImage), cancellationToken);

            await _mediator.Publish(new InteriorDesignSectionImageChangedEvent(), cancellationToken);

            return Ok(newAddedImage);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Pages.ChangeInteriorDesignMobileImage)]
        public async Task<IActionResult> ChangeInteriorDesignMoblieImage(ChangeInteriorDesignSectionMobileImageDto newImage, CancellationToken cancellationToken = default)
        {
            var newAddedImage = await _mediator.Send(new ChangeInteriorDesignSectionMobileImage.Request(newImage), cancellationToken);

            await _mediator.Publish(new InteriorDesignSectionMobileImageChangedEvent(), cancellationToken);

            return Ok(newAddedImage);
        }
    }
}
