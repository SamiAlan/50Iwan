using Iwan.Shared.Dtos.Media;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Compositions
{
    public class CompositionImageDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("originalImage")]
        public ImageDto OriginalImage { get; set; }

        [JsonPropertyName("mediumImage")]
        public ImageDto MediumImage { get; set; }

        [JsonPropertyName("mobileImage")]
        public ImageDto MobileImage { get; set; }
    }
}
