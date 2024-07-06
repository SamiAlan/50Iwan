using Iwan.Server.Commands.Sales.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Options.Sales;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class BillsController : BaseAdminApiController
    {
        public BillsController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer)
            : base(mediator, stringLocalizer) { }

        //[HttpGet]
        //[Route(Routes.Api.Admin.Bills.BASE)]
        //public async Task<IActionResult> Get(GetBillsOptions options, CancellationToken cancellationToken = default)
        //{
        //    //var bills = await _mediator.Send(new GetBills.Request(options), cancellationToken);

        //    //return Ok(bills);
        //}

        //[HttpGet]
        //[Route(Routes.Api.Admin.Bills.GetBill)]
        //public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        //{
        //    //var bills = await _mediator.Send(new GetBill.Request(id), cancellationToken);

        //    //return Ok(bills);
        //}

        //[HttpPost]
        //[Route(Routes.Api.Admin.Bills.BASE)]
        //public async Task<IActionResult> Post(AddBillDto request, CancellationToken cancellationToken = default)
        //{
        //    var bill = await _mediator.Send(new AddBill.Request(request), cancellationToken);

        //    //await _mediator.Publish(new BillAddedEvent(bill.Id), cancellationToken);

        //    return Ok(bill);
        //}

        //[HttpPut]
        //[Route(Routes.Api.Admin.Bills.BASE)]
        //public async Task<IActionResult> Edit(EditBillDto request, CancellationToken cancellationToken = default)
        //{
        //    var bill = await _mediator.Send(new EditBill.Request(request), cancellationToken);

        //    //await _mediator.Publish(new BillEditedEvent(bill.Id), cancellationToken);

        //    return Ok(bill);
        //}

        //[HttpDelete]
        //[Route(Routes.Api.Admin.Bills.Delete)]
        //public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        //{
        //    await _mediator.Send(new DeleteBill.Request(id), cancellationToken);

        //    //await _mediator.Publish(new BillDeletedEvent(id), cancellationToken);

        //    return Ok();
        //}

        //[HttpPost]
        //[Route(Routes.Api.Admin.Bills.BASE_BILL_ITEM)]
        //public async Task<IActionResult> AddBillItem(AddBillItemDto request, CancellationToken cancellationToken = default)
        //{
        //    var billItem = await _mediator.Send(new AddBillItem.Request(request), cancellationToken);

        //    //await _mediator.Publish(new BillItemAddedEvent(billItem.Id), cancellationToken);

        //    return Ok(billItem);
        //}

        //[HttpPut]
        //[Route(Routes.Api.Admin.Bills.BASE)]
        //public async Task<IActionResult> EditBillItem(EditBillItemDto request, CancellationToken cancellationToken = default)
        //{
        //    var billItem = await _mediator.Send(new EditBillItem.Request(request), cancellationToken);

        //    //await _mediator.Publish(new BillItemEditedEvent(billItem.Id), cancellationToken);

        //    return Ok(billItem);
        //}

        //[HttpDelete]
        //[Route(Routes.Api.Admin.Bills.DeleteBillItem)]
        //public async Task<IActionResult> DeleteBillItem(string id, CancellationToken cancellationToken = default)
        //{
        //    await _mediator.Send(new RemoveBillItem.Request(id), cancellationToken);

        //    //await _mediator.Publish(new BillItemRemovedEvent(id), cancellationToken);

        //    return Ok();
        //}
    }
}
