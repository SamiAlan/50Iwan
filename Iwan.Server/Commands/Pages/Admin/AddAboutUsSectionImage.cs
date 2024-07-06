using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class AddAboutUsSectionImage
    {
        public record Request(AddAboutUsSectionImageDto Image) : IRequest<AboutUsSectionImageDto>;

        public class Handler : IRequestHandler<Request, AboutUsSectionImageDto>
        {
            protected readonly IPagesService _pagesService;
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesService pagesService, IPagesQueryService pagesQueryService)
            {
                _pagesService = pagesService;
                _pagesQueryService = pagesQueryService;
            }

            public async Task<AboutUsSectionImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var image = await _pagesService.AddImageAsync(request.Image, cancellationToken);

                return await _pagesQueryService.GetAboutUsSectionImageDetailsAsync(image.Id, cancellationToken);
            }
        }
    }
}
