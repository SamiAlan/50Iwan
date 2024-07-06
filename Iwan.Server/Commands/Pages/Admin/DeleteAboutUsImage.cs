using Iwan.Server.Services.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class DeleteAboutUsImage
    {
        public record Request(string ImageId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IPagesService _pagesService;

            public Handler(IPagesService pagesService)
            {
                _pagesService = pagesService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _pagesService.DeleteAboutUsImageAsync(request.ImageId, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
