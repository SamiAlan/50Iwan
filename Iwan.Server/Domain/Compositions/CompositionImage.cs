using Iwan.Server.Domain.Media;

namespace Iwan.Server.Domain.Compositions
{
    public class CompositionImage : BaseEntity
    {
        public string CompositionId { get; set; }
        public string OriginalImageId { get; set; }
        public string MediumImageId { get; set; }
        public string MobileImageId { get; set; }

        public virtual Composition Composition { get; set; }
        public virtual Image OriginalImage { get; set; }
        public virtual Image MediumImage { get; set; }
        public virtual Image MobileImage { get; set; }
    }
}
