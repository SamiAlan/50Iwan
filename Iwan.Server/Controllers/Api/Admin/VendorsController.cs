using Iwan.Server.Commands.Vendors.Admin;
using Iwan.Server.Events.Vendors;
using Iwan.Server.Queries.Vendors.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Options.Vendors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class VendorsController : BaseAdminApiController
    {
        public VendorsController(IMediator mediator, IStringLocalizer<Localization> localizer)
            : base(mediator, localizer) { }

        [HttpGet]
        [Route(Routes.Api.Admin.Vendors.BASE)]
        public async Task<IActionResult> Get([FromQuery] GetVendorsOptions options, CancellationToken cancellationToken = default)
        {
            var vendors = await _mediator.Send(new GetVendors.Request(options), cancellationToken);

            return Ok(vendors);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Vendors.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var vendors = await _mediator.Send(new GetAllVendors.Request(), cancellationToken);

            return Ok(vendors);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Vendors.GetVendor)]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            var vendor = await _mediator.Send(new GetVendor.Request(id), cancellationToken);

            return Ok(vendor);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Vendors.BASE)]
        public async Task<IActionResult> Post(AddVendorDto request, CancellationToken cancellationToken = default)
        {
            var vendor = await _mediator.Send(new AddVendor.Request(request), cancellationToken);

            await _mediator.Publish(new VendorAddedEvent(vendor.Id), cancellationToken);

            return Ok(vendor);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Vendors.BASE)]
        public async Task<IActionResult> Put(EditVendorDto request, CancellationToken cancellationToken = default)
        {
            var vendor = await _mediator.Send(new EditVendor.Request(request), cancellationToken);

            await _mediator.Publish(new VendorEditedEvent(vendor.Id), cancellationToken);

            return Ok(vendor);
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Vendors.Delete)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteVendor.Request(id), cancellationToken);

            await _mediator.Publish(new VendorDeletedEvent(id), cancellationToken);

            return Ok();
        }
    }
}
