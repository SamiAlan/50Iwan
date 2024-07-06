using Iwan.Server.Domain.Media;

namespace Iwan.Server.Domain.Settings
{
    public class WatermarkSettings : BaseEntity
    {
        public bool ShouldAddWatermark { get; set; } = true;
        public float Opacity { get; set; } = .7f;
        public string WatermarkImageId { get; set; }

        public virtual Image WatermarkImage { get; set; }
    }
}
