using Iwan.Shared.Dtos.Media;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Sliders
{
    public class SliderImageDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("mediumImage")]
        public ImageDto MediumImage { get; set; }

        [JsonPropertyName("mobileImage")]
        public ImageDto MobileImage { get; set; }
    }
}
