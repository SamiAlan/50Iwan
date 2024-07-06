using Iwan.Server.Domain.Media;

namespace Iwan.Server.Domain.Pages
{
    public class InteriorDesignSectionImage : BaseEntity
    {
        public string InteriorDesignSectionId { get; set; }
        public string MainImageId { get; set; }
        public string MobileImageId { get; set; }

        public virtual InteriorDesignSection InteriorDesignSection { get; set; }
        public virtual Image MainImage { get; set; }
        public virtual Image MobileImage { get; set; }
    }
}
