using Iwan.Server.Services.Sliders;
using Iwan.Shared.Dtos.Sliders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Sliders.Admin
{
    public class GetSliderImage
    {
        public record Request(string Id) : IRequest<SliderImageDto>;

        public class Handler : IRequestHandler<Request, SliderImageDto>
        {
            protected readonly IQuerySliderService _querySliderService;


            public Handler(IQuerySliderService sliderService)
            {
                _querySliderService = sliderService;
            }

            public async Task<SliderImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _querySliderService.GetSliderImageDetailsAsync(request.Id, cancellationToken);
            }
        }
    }
}
