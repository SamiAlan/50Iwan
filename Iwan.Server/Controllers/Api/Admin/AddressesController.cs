using Iwan.Server.Commands.Common.Admin;
using Iwan.Server.Events.Common;
using Iwan.Server.Queries.Common.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class AddressesController : BaseAdminApiController
    {
        public AddressesController(IMediator mediator, IStringLocalizer<Localization> localizer)
            : base(mediator, localizer) { }

        [HttpGet]
        [Route(Routes.Api.Admin.Addresses.GetAddress)]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            var address = await _mediator.Send(new GetAddress.Request(id), cancellationToken);

            return Ok(address);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Addresses.BASE)]
        public async Task<IActionResult> Edit(EditAddressDto request, CancellationToken cancellationToken = default)
        {
            var address = await _mediator.Send(new EditAddress.Request(request), cancellationToken);

            await _mediator.Publish(new AddressEditedEvent(address.Id), cancellationToken);

            return Ok(address);
        }
    }
}
