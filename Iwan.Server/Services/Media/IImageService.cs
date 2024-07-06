using Iwan.Server.Domain.Media;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Media
{
    public interface IImageService
    {
        Task<TempImage> UploadTempImageAsync(IFormFile imageFile, bool useTXFileManager = true, CancellationToken cancellationToken = default);
        
        Task<TempImage> UploadTempImageAsync(Stream readStream, string fileName, long length, string contentType, bool useTXFileManager = true, CancellationToken cancellationToken = default);
        
        Task DeleteTempImageAsync(string tempImageId, bool deleteImageFile = true, bool useTXFileManager = true, CancellationToken cancellation = default);

        Task DeleteImagesAsync(IEnumerable<Image> images, bool deleteImageFiles = true, bool useTXFileManager = true, CancellationToken cancellationToken = default);

        void DeleteImageFile(Image image, bool useTXFileManager = true);

        void DeleteImagesFiles(IEnumerable<Image> images, bool useTXFileManager = true);

        void DeleteImagesFiles(IEnumerable<TempImage> images, bool useTXFileManager = true);

        void DeleteImagesFiles(bool useTXFileManager = true, params Image[] images);

        Task<Image> ResizeAndAddVersionOfImageAsync(string mainImageFilePath, string virtualPath, string extension, string postfix, int width, int height, bool useTXFileManager = true, CancellationToken cancellationToken = default);
        Task<Image> ResizeAndAddVersionOfImageWithWatermarkAsync(string mainImageFilePath, string virtualPath, string extension, string postfix, int width, int height, byte[] watermarkImageBytes, float opacity, bool useTXFileManager = true, CancellationToken cancellationToken = default);
    }
}
