using Iwan.Server.Domain.Sliders;
using Iwan.Shared.Dtos.Sliders;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sliders
{
    public interface ISliderService
    {
        Task<SliderImage> AddSliderImageAsync(AddSliderImageDto imageDto, CancellationToken cancellationToken = default);

        Task DeleteSliderImageAsync(string sliderImageId, bool deleteImageFiles = true, CancellationToken cancellationToken = default);

        Task<SliderImage> EditSliderImageAsync(EditSliderImageDto editedImage, CancellationToken cancellationToken);
    }
}
