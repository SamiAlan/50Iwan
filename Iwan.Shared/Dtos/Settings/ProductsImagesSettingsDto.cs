using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Settings
{
    public class ProductsImagesSettingsDto
    {
        [JsonPropertyName("mediumImageWidth")]
        public int MediumImageWidth { get; set; }

        [JsonPropertyName("mediumImageHeight")]
        public int MediumImageHeight { get; set; }

        [JsonPropertyName("smallImageWidth")]
        public int SmallImageWidth { get; set; }

        [JsonPropertyName("smallImageHeight")]
        public int SmallImageHeight { get; set; }

        [JsonPropertyName("mobileImageWidth")]
        public int MobileImageWidth { get; set; }

        [JsonPropertyName("mobileImageHeight")]
        public int MobileImageHeight { get; set; }
    }
}
