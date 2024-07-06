using Iwan.Server.Models.Common;
using Iwan.Server.Models.Media;

namespace Iwan.Server.Models.Products
{
    public class ProductMainImageViewModel : EntityBackgroundColorViewModel
    {
        public string Id { get; set; }
        public ImageViewModel OriginalImage { get; set; }
        public ImageViewModel MediumImage { get; set; }
        public ImageViewModel SmallImage { get; set; }
        public ImageViewModel MobileImage { get; set; }
    }
}
