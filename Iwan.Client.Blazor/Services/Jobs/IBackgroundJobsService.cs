using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Jobs
{
    public interface IBackgroundJobsService
    {
        Task StartWatermarkingJobAsync(CancellationToken cancellationToken = default);
        Task StartUnWatermarkingJobAsync(CancellationToken cancellationToken = default);
        Task StartResizingProductsImagesAsync(CancellationToken cancellationToken = default);
        Task StartResizingCategoriesImagesAsync(CancellationToken cancellationToken = default);
        Task StartResizingCompositionsImagesAsync(CancellationToken cancellationToken = default);
        Task StartResizingSliderImagesAsync(CancellationToken cancellationToken = default);
        Task StartResizingAboutUsImagesAsync(CancellationToken cancellationToken = default);
    }
}
