using Iwan.Server.Constants;
using Iwan.Server.Services.Sales;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Sales.Admin
{
    // Edits the basic information of the bill
    public class EditBill
    {
        public record Request(EditBillDto EditedBill) : IRequest<BillDto>;

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
                    var bill = await _billService.EditBillAsync(request.EditedBill, cancellationToken);

                    return await _queryBillService.GetBillDetailsAsync(bill.Id, cancellationToken);
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Bills.ErrorEditingBill); }
            }
        }
    }
}
