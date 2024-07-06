using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Options.SliderImages;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sliders
{
    public interface IQuerySliderService
    {
        Task<PagedDto<SliderImageDto>> GetSliderImagesDetailsAsync(GetSliderImagesOptions options, CancellationToken cancellationToken = default);
        Task<SliderImageDto> GetSliderImageDetailsAsync(string sliderImageId, CancellationToken cancellationToken = default);
    }
}
