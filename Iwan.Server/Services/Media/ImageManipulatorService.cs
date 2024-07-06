using Iwan.Server.Infrastructure.Files;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Media
{
    [Injected(ServiceLifetime.Scoped, typeof(IImageManipulatorService))]
    public class ImageManipulatorService : IImageManipulatorService
    {
        protected readonly IFileProvider _fileProvider;

        public ImageManipulatorService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public async Task AddWatermarkToImageAsync(string imagePath, byte[] watermarkImageBytes, float opacity = .7f, CancellationToken cancellationToken = default)
        {
            using (var image = await Image.LoadAsync(imagePath, cancellationToken))
            using (var watermarkImage = Image.Load(watermarkImageBytes))
            {
                watermarkImage.Mutate(c => c.Resize(image.Size()));
                image.Mutate(c => c.DrawImage(watermarkImage, opacity));
                await image.SaveAsync(imagePath, cancellationToken);
            }
        }

        public async Task<byte[]> ResizeAndGetBytesAsync(string imagePath, int width, int height, CancellationToken cancellationToken = default)
        {
            using (var image = await Image.LoadAsync(imagePath, cancellationToken))
            using (var memoryStream = new MemoryStream())
            {
                var originalImageWidthRatio = (float)image.Width / image.Height;
                var originalImageHeightRatio = (float)image.Height / image.Width;
                // var newSizeWidthRatio = (float)width / height;

                var newHeight = 0;
                var newWidth = 0;

                newWidth = width;
                if (originalImageWidthRatio > 1)
                {
                    newHeight = (int)(newWidth / originalImageWidthRatio);
                }
                else
                {
                    newHeight = (int)(newWidth * originalImageHeightRatio);
                }
                //if (newSizeWidthRatio < 1 && originalImageWidthRatio < 1)
                //{
                //    newHeight = height;
                //    newWidth = (int)(newHeight * newSizeWidthRatio);
                //}
                //else if (newSizeWidthRatio > 1 && originalImageWidthRatio < 1)
                //{
                //    newWidth = width;
                //    newHeight = (int)(newWidth / newSizeWidthRatio);
                //}
                //else if (newSizeWidthRatio < 1 && originalImageWidthRatio > 1)
                //{
                //    newWidth = width;
                //    newHeight = (int)(newWidth * newSizeWidthRatio);
                //}
                //else
                //{
                //    newHeight = height;
                //    newWidth = (int)(newHeight / newSizeWidthRatio);
                //}

                image.Mutate(c => c.Resize(newWidth, newHeight));
                await image.SaveAsync(memoryStream, GetFormatFromExtension(_fileProvider.GetFileExtension(imagePath)), cancellationToken);
                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> ResizeAndAddWatermarkAndGetBytesAsync(string imagePath, int width, int height, byte[] watermarkImageBytes, float opacity = .7f, CancellationToken cancellationToken = default)
        {
            using (var image = await Image.LoadAsync(imagePath, cancellationToken))
            using (var memoryStream = new MemoryStream())
            using (var watermarkImage = Image.Load(watermarkImageBytes))
            {
                watermarkImage.Mutate(c => c.Resize(width, height));
                image.Mutate(c => c.Resize(width, height).DrawImage(watermarkImage, opacity));
                await image.SaveAsync(memoryStream, GetFormatFromExtension(_fileProvider.GetFileExtension(imagePath)), cancellationToken);
                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> GetBytesAsync(string imagePath, CancellationToken cancellationToken = default)
        {
            using (var image = await Image.LoadAsync(imagePath, cancellationToken))
            using (var memoryStream = new MemoryStream())
            {
                await image.SaveAsync(memoryStream, GetFormatFromExtension(_fileProvider.GetFileExtension(imagePath)), cancellationToken);
                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> GetBytesAsync(Domain.Media.Image image, CancellationToken cancellationToken = default)
        {
            var path = _fileProvider.CombineWithRoot(image.VirtualPath, image.FileName);

            return await GetBytesAsync(path, cancellationToken);
        }

        public async Task ResizeFromOriginalAndReplaceImageAsync(Domain.Media.Image image, int width, int height, byte[] originalImageBytes, CancellationToken cancellationToken = default)
        {
            var path = _fileProvider.CombineWithRoot(image.VirtualPath, image.FileName);
            using (var i = Image.Load(originalImageBytes))
            {
                var originalImageWidthRatio = (float)i.Width / i.Height;
                var originalImageHeightRatio = (float)i.Height / i.Width;
                // var newSizeWidthRatio = (float)width / height;

                var newHeight = 0;
                var newWidth = 0;

                newWidth = width;
                if (originalImageWidthRatio > 1)
                {
                    newHeight = (int)(newWidth / originalImageWidthRatio);
                }
                else
                {
                    newHeight = (int)(newWidth * originalImageHeightRatio);
                }


                //if (newSizeWidthRatio < 1 && originalImageWidthRatio < 1)
                //{
                //    newHeight = height;
                //    newWidth = (int)(newHeight * newSizeWidthRatio);
                //}
                //else if (newSizeWidthRatio > 1 && originalImageWidthRatio < 1)
                //{
                //    newWidth = width;
                //    newHeight = (int)(newWidth / newSizeWidthRatio);
                //}
                //else if (newSizeWidthRatio < 1 && originalImageWidthRatio > 1)
                //{
                //    newWidth = width;
                //    newHeight = (int)(newWidth * newSizeWidthRatio);
                //}
                //else
                //{
                //    newHeight = height;
                //    newWidth = (int)(newHeight / newSizeWidthRatio);
                //}

                i.Mutate(p => p.Resize(newWidth, newHeight));
                await i.SaveAsync(path, cancellationToken);
            }
        }

        public async Task AddWatermarkToImagesAsync(IEnumerable<Domain.Media.Image> images, byte[] watermarkImageBytes, float opacity = .7f, CancellationToken cancellationToken = default)
        {
            foreach (var image in images)
            {
                var path = _fileProvider.CombineWithRoot(image.VirtualPath, image.FileName);
                await AddWatermarkToImageAsync(path, watermarkImageBytes, opacity, cancellationToken);
            }
        }

        #region Helpers

        private static IImageFormat GetFormatFromExtension(string extension)
        {
            return extension switch
            {
                ".png" => PngFormat.Instance,
                ".PNG" => PngFormat.Instance,
                ".jpg" => JpegFormat.Instance,
                ".JPG" => JpegFormat.Instance,
                ".jpeg" => JpegFormat.Instance,
                ".JPEG" => JpegFormat.Instance,
                _ => PngFormat.Instance
            };
        }

        #endregion
    }
}
