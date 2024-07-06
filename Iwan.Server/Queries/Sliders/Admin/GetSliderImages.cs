using Iwan.Server.Services.Sliders;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Options.SliderImages;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Sliders.Admin
{
    public class GetSliderImages
    {
        public record Request(GetSliderImagesOptions Options) : IRequest<PagedDto<SliderImageDto>>;

        public class Handler : IRequestHandler<Request, PagedDto<SliderImageDto>>
        {
            protected readonly IQuerySliderService _querySliderService;


            public Handler(IQuerySliderService sliderService)
            {
                _querySliderService = sliderService;
            }

            public async Task<PagedDto<SliderImageDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _querySliderService.GetSliderImagesDetailsAsync(request.Options, cancellationToken);
            }
        }
    }
}
