using Iwan.Server.Commands.Media.Admin;
using Iwan.Server.Events.Media;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Media;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;


namespace Iwan.Server.Controllers.Api.Admin
{
    public class ImagesController : BaseAdminApiController
    {
        public ImagesController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer)
            : base(mediator, stringLocalizer) { }

        [HttpPost]
        [Route(Routes.Api.Admin.Images.BASE_TEMP)]
        public async Task<IActionResult> Post([FromForm]UploadTempImageDto request, CancellationToken cancellationToken = default)
        {
            var tempImage = await _mediator.Send(new UploadTempImage.Request(request.Image), cancellationToken);

            await _mediator.Publish(new TempImageUploadedEvent(tempImage.Id), cancellationToken);

            return Ok(tempImage);
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Images.DeleteTemp)]
        public async Task<IActionResult> DeleteImage(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteTempImage.Request(id), cancellationToken);

            await _mediator.Publish(new TempImageDeletedEvent(id), cancellationToken);

            return Ok();
        }
    }
}
