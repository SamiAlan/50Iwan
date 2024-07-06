using System.Threading.Tasks;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Options.SliderImages;
using Iwan.Shared.Dtos;
using System.Threading;

namespace Iwan.Client.Blazor.Services.SliderImages
{
    public interface ISliderImageService
    {
        Task<SliderImageDto> GetSliderImageAsync(string id, CancellationToken cancellation = default);
        Task<PagedDto<SliderImageDto>> GetSliderImagesAsync(GetSliderImagesOptions options, CancellationToken cancellation = default);
        Task AddSliderImageAsync(AddSliderImageDto sliderImage, CancellationToken cancellationToken = default);
        Task EditSliderImageAsync(EditSliderImageDto sliderImage, CancellationToken cancellationToken = default);
        Task DeleteSliderImageAsync(string id, CancellationToken cancellationToken = default);
    }
}
