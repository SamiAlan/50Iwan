using Iwan.Server.Domain.Sliders;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Sliders
{
    public interface ISliderImageRepository : IRepository<SliderImage>
    {
        Task<IEnumerable<SliderImage>> GetWithImagesAsync(CancellationToken cancellationToken = default);
        Task<SliderImage> GetWithImagesAsync(string sliderImageId, CancellationToken cancellationToken = default);
    }
}
