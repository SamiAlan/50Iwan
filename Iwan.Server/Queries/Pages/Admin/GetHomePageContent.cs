using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Pages.Admin
{
    public class GetHomePageContent
    {
        public record Request : IRequest<HomePageContentDto>;

        public class Handler : IRequestHandler<Request, HomePageContentDto>
        {
            protected readonly IPagesQueryService _homeQueryService;

            public Handler(IPagesQueryService homeQueryService)
            {
                _homeQueryService = homeQueryService;
            }

            public async Task<HomePageContentDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _homeQueryService.GetHomePageContentDetailsAsync(cancellationToken);
            }
        }
    }
}
