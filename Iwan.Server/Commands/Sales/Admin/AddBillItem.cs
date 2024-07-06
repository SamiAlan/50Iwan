using Iwan.Server.Constants;
using Iwan.Server.Services.Sales;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Sales.Admin
{
    // Adds an item to an already saved bill
    public class AddBillItem
    {
        public record Request(AddBillItemDto BillItem) : IRequest<BillItemDto>;

        public class Handler : IRequestHandler<Request, BillItemDto>
        {
            protected readonly IBillService _billService;
            protected readonly IQueryBillService _queryBillService;

            public Handler(IBillService billService, IQueryBillService queryillService)
            {
                _billService = billService;
                _queryBillService = queryillService;
            }

            public async Task<BillItemDto> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var billItem = await _billService.AddBillItemAsync(request.BillItem.BillId, request.BillItem, cancellationToken);

                    return await _queryBillService.GetBillItemDetailsAsync(billItem.Id, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Bills.ErrorAddingBillItem); }
            }
        }
    }
}
