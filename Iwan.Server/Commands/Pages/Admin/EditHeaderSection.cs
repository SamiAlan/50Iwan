using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class EditHeaderSection
    {
        public record Request(EditHeaderSectionDto Section) : IRequest<HeaderSectionDto>;

        public class Handler : IRequestHandler<Request, HeaderSectionDto>
        {
            protected readonly IPagesService _pagesService;
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesService pagesService, IPagesQueryService pagesQueryService)
            {
                _pagesService = pagesService;
                _pagesQueryService = pagesQueryService;
            }

            public async Task<HeaderSectionDto> Handle(Request request, CancellationToken cancellationToken)
            {
                await _pagesService.EditSectionAsync(request.Section, cancellationToken);

                return await _pagesQueryService.GetHeaderSectionDetailsAsync(cancellationToken);
            }
        }
    }
}
