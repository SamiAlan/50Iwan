using Iwan.Server.Models.Common;
using Iwan.Server.Models.Media;

namespace Iwan.Server.Models.Catalog
{
    public class CategoryImageModel : EntityBackgroundColorViewModel
    {
        public ImageViewModel OriginalImage { get; set; }
        public ImageViewModel MediumImage { get; set; }
        public ImageViewModel MobileImage { get; set; }
    }
}
