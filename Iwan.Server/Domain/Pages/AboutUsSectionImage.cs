using Iwan.Server.Domain.Media;

namespace Iwan.Server.Domain.Pages
{
    public class AboutUsSectionImage : BaseEntity
    {
        public string AboutUsSectionId { get; set; }
        public string OriginalImageId { get; set; }
        public string MediumImageId { get; set; }
        public string MobileImageId { get; set; }

        public virtual AboutUsSection AboutUsSection { get; set; }
        public virtual Image OriginalImage { get; set; }
        public virtual Image MediumImage { get; set; }
        public virtual Image MobileImage { get; set; }
    }
}
