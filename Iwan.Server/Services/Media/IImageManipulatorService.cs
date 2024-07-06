using Iwan.Server.Domain.Media;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Media
{
    public interface IImageManipulatorService
    {
        Task AddWatermarkToImagesAsync(IEnumerable<Domain.Media.Image> images, byte[] watermarkImageBytes, float opacity = .7f, CancellationToken cancellationToken = default);
        Task AddWatermarkToImageAsync(string imagePath, byte[] watermarkImageBytes, float opacity = .7f, CancellationToken cancellationToken = default);
        Task<byte[]> ResizeAndGetBytesAsync(string imagePath, int width, int height, CancellationToken cancellationToken = default);
        Task ResizeFromOriginalAndReplaceImageAsync(Image image, int width, int height, byte[] originalImageBytes, CancellationToken cancellationToken = default);
        Task<byte[]> GetBytesAsync(Image image, CancellationToken cancellationToken = default);
        Task<byte[]> GetBytesAsync(string imagePath, CancellationToken cancellationToken = default);
        Task<byte[]> ResizeAndAddWatermarkAndGetBytesAsync(string imagePath, int width, int height, byte[] watermarkImageBytes, float opacity = .7f, CancellationToken cancellationToken = default);
    }
}
