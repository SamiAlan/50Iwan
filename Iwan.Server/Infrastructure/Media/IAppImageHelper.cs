using Iwan.Server.Domain.Media;
using Iwan.Server.Models.Media;
using Iwan.Shared.Dtos.Media;

namespace Iwan.Server.Infrastructure.Media
{
    public interface IAppImageHelper
    {
        string GenerateUrlToResource(string virtualPath, string filename);
        string GenerateUrlToImage(Image image);
        ImageDto GenerateImageDto(Image image);
        ImageViewModel GenerateImageModel(Image image);
        TempImageDto GenerateImageDto(TempImage image);
    }
}
