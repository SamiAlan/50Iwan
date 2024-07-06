using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.RealTime
{
    public interface IRealTimeNotifierImagesManipulatorService
    {
        Task RemoveWatermarkFromImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
        Task AddWatermarkToImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
        Task ResizeProductsImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
        Task ResizeCategoriesImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
        Task ResizeCompositionsImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
        Task ResizeSliderImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
        Task ResizeAboutUsImagesAsync(PerformContext context, CancellationToken cancellationToken = default);
    }
}
