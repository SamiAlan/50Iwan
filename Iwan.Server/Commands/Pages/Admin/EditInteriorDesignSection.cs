using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class EditInteriorDesignSection
    {
        public record Request(EditInteriorDesignSectionDto Section) : IRequest<InteriorDesignSectionDto>;

        public class Handler : IRequestHandler<Request, InteriorDesignSectionDto>
        {
            protected readonly IPagesService _pagesService;
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesService pagesService, IPagesQueryService pagesQueryService)
            {
                _pagesService = pagesService;
                _pagesQueryService = pagesQueryService;
            }

            public async Task<InteriorDesignSectionDto> Handle(Request request, CancellationToken cancellationToken)
            {
                await _pagesService.EditSectionAsync(request.Section, cancellationToken);

                return await _pagesQueryService.GetInteriorDesignSectionDetailsAsync(cancellationToken);
            }
        }
    }
}
