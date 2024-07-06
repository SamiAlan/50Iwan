using Iwan.Server.Domain.Media;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Models.Media;
using Iwan.Server.Options;
using Iwan.Shared.Dtos.Media;
using Microsoft.Extensions.DependencyInjection;

namespace Iwan.Server.Infrastructure.Media
{
    [Injected(ServiceLifetime.Scoped, typeof(IAppImageHelper))]
    public class AppImageHelper : IAppImageHelper
    {
        protected readonly PathsOptions _pathsOptions;

        public AppImageHelper(PathsOptions pathsOptions)
        {
            _pathsOptions = pathsOptions;
        }

        public string GenerateUrlToResource(string virtualPath, string filename)
        {
            virtualPath = virtualPath.Replace("\\", "/");

            return $"{_pathsOptions.MainUrl}/{virtualPath}/{filename}";
        }

        public string GenerateUrlToImage(Image image)
        {
            return GenerateUrlToResource(image.VirtualPath, image.FileName);
        }

        public ImageDto GenerateImageDto(Image image)
        {
            return new ImageDto
            {
                Id = image.Id,
                Url = GenerateUrlToImage(image)
            };
        }

        public TempImageDto GenerateImageDto(TempImage image)
        {
            return new TempImageDto
            {
                Id = image.Id,
                OriginalImageUrl = GenerateUrlToResource(image.VirtualPath, image.FileName),
                SmallImageUrl = GenerateUrlToResource(image.VirtualPath, image.SmallVersionFileName)
            };
        }

        public ImageViewModel GenerateImageModel(Image image)
        {
            return new ImageViewModel
            {
                Id = image.Id,
                Url = GenerateUrlToImage(image)
            };
        }
    }
}
