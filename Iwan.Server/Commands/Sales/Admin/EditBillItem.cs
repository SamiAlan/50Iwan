using Iwan.Server.Constants;
using Iwan.Server.Services.Sales;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Sales.Admin
{
    // Edit a bill item in an already saved bill
    public class EditBillItem
    {
        public record Request(EditBillItemDto EditedBillItem) : IRequest<BillItemDto>;

        public class Handler : IRequestHandler<Request, BillItemDto>
        {
            protected readonly IBillService _billService;
            protected readonly IQueryBillService _queryBillService;

            public Handler(IBillService billService, IQueryBillService queryBillService)
            {
                _billService = billService;
                _queryBillService = queryBillService;
            }

            public async Task<BillItemDto> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var billItem = await _billService.EditBillItemAsync(request.EditedBillItem, cancellationToken);

                    return await _queryBillService.GetBillItemDetailsAsync(billItem.Id, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Bills.ErrorEditingBillItem); }
            }
        }
    }
}
