using Iwan.Server.Constants;
using Iwan.Server.Services.Sales;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Sales.Admin
{
    // Clients should store product they query about for proper stock warnings and so on..
    // Adds a bill with all its items and other info
    public class AddBill
    {
        public record Request(AddBillDto Bill) : IRequest<BillDto>;

        public class Handler : IRequestHandler<Request, BillDto>
        {
            protected readonly IBillService _billService;
            protected readonly IQueryBillService _queryBillService;

            public Handler(IBillService billService, IQueryBillService queryBillService)
            {
                _billService = billService;
                _queryBillService = queryBillService;
            }

            public async Task<BillDto> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    var bill = await _billService.AddBillAsync(request.Bill, cancellationToken);

                    return await _queryBillService.GetBillDetailsAsync(bill.Id, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Bills.ErrorAddingBill); }
            }
        }
    }
}
