using Iwan.Shared.Dtos.Media;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Settings
{
    public class WatermarkSettingsDto
    {
        [JsonPropertyName("shouldAddWatermark")]
        public bool ShouldAddWatermark { get; set; }

        [JsonPropertyName("watermarkImage")]
        public WatermarkImageDto WatermarkImage { get; set; }

        [JsonPropertyName("opacity")]
        public float Opacity { get; set; }
    }

    public class WatermarkImageDto
    {
        [JsonPropertyName("image")]
        public ImageDto Image { get; set; }
    }
}
