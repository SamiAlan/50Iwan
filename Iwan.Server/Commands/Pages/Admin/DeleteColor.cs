using Iwan.Server.Services.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class DeleteColor
    {
        public record Request(string ColorId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IPagesService _pagesService;
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesService pagesService, IPagesQueryService pagesQueryService)
            {
                _pagesService = pagesService;
                _pagesQueryService = pagesQueryService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _pagesService.DeleteColorAsync(request.ColorId, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
