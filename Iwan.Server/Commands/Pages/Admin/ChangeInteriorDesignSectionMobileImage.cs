using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class ChangeInteriorDesignSectionMobileImage
    {
        public record Request(ChangeInteriorDesignSectionMobileImageDto Image) : IRequest<InteriorDesignSectionImageDto>;

        public class Handler : IRequestHandler<Request, InteriorDesignSectionImageDto>
        {
            protected readonly IPagesService _pagesService;
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesService pagesService, IPagesQueryService pagesQueryService)
            {
                _pagesService = pagesService;
                _pagesQueryService = pagesQueryService;
            }

            public async Task<InteriorDesignSectionImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                await _pagesService.ChangeMobileImageAsync(request.Image, cancellationToken);

                return await _pagesQueryService.GetInteriorDesignSectionImageDetailsAsync(cancellationToken);
            }
        }
    }
}
