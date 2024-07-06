using Iwan.Server.Constants;
using Iwan.Server.Services.Sales;
using Iwan.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Sales.Admin
{
    // Remove a bill item from an already saved bill (re-stock)
    public class RemoveBillItem
    {
        public record Request(string BillItemId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IBillService _billService;

            public Handler(IBillService billService)
            {
                _billService = billService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    await _billService.RemoveBillItemAsync(request.BillItemId, cancellationToken);

                    return Unit.Value;
                }
                catch (BaseException) { throw; }
                catch { throw new ServerErrorException(Responses.Bills.ErrorRemovingBillItem); }
            }
        }
    }
}
