using Iwan.Server.Commands.Compositions.Admin;
using Iwan.Server.Events.Compositions;
using Iwan.Server.Queries.Compositions;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Options.Compositions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class CompositionsController : BaseAdminApiController
    {
        public CompositionsController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer)
            : base(mediator, stringLocalizer) { }

        [HttpGet]
        [Route(Routes.Api.Admin.Compositions.BASE)]
        public async Task<IActionResult> Get([FromQuery]GetCompositionsOptions options, CancellationToken cancellationToken = default)
        {
            var compositions = await _mediator.Send(new GetCompositions.Request(options), cancellationToken);

            return Ok(compositions);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Compositions.GetComposition)]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            var composition = await _mediator.Send(new GetComposition.Request(id), cancellationToken);

            return Ok(composition);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Compositions.BASE)]
        public async Task<IActionResult> Post(AddCompositionDto request, CancellationToken cancellationToken = default)
        {
            var composition = await _mediator.Send(new AddComposition.Request(request), cancellationToken);

            await _mediator.Publish(new CompositionAddedEvent(composition.Id), cancellationToken);

            return Ok(composition);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Compositions.BASE)]
        public async Task<IActionResult> Edit(EditCompositionDto request, CancellationToken cancellationToken = default)
        {
            var composition = await _mediator.Send(new EditComposition.Request(request), cancellationToken);

            await _mediator.Publish(new CompositionEditedEvent(composition.Id), cancellationToken);

            return Ok(composition);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Compositions.EditImage)]
        public async Task<IActionResult> Edit(ChangeCompositionImageDto request, CancellationToken cancellationToken = default)
        {
            var composition = await _mediator.Send(new ChangeCompositionImage.Request(request),
                cancellationToken);

            await _mediator.Publish(new CompositionImageChangedEvent(request.CompositionId), cancellationToken);

            return Ok(composition);
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Compositions.Delete)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteComposition.Request(id), cancellationToken);

            await _mediator.Publish(new CompositionDeletedEvent(id), cancellationToken);

            return Ok();
        }
    }
}
