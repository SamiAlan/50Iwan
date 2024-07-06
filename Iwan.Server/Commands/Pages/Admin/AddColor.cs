using Iwan.Server.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Pages.Admin
{
    public class AddColor
    {
        public record Request(AddColorDto Color) : IRequest<ColorDto>;

        public class Handler : IRequestHandler<Request, ColorDto>
        {
            protected readonly IPagesService _pagesService;
            protected readonly IPagesQueryService _pagesQueryService;

            public Handler(IPagesService pagesService, IPagesQueryService pagesQueryService)
            {
                _pagesService = pagesService;
                _pagesQueryService = pagesQueryService;
            }

            public async Task<ColorDto> Handle(Request request, CancellationToken cancellationToken)
            {
                var color = await _pagesService.AddColorAsync(request.Color, cancellationToken);

                return new ColorDto
                {
                    Id = color.Id,
                    ColorCode = color.ColorCode,
                };
            }
        }
    }
}
