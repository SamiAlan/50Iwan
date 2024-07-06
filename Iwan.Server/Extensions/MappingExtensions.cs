using Iwan.Server.Models.Media;
using Iwan.Shared.Dtos.Media;

namespace Iwan.Server.Extensions
{
    public static class MappingExtensions
    {
        public static ImageViewModel ToViewModel(this ImageDto image)
        {
            return new ImageViewModel
            {
                Id = image.Id,
                Url = image.Url,
            };
        }
    }
}
