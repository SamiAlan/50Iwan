using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Media
{
    public class TempImageDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("originalImageUrl")]
        public string OriginalImageUrl { get; set; }

        [JsonPropertyName("smallImageUrl")]
        public string SmallImageUrl { get; set; }
    }
}
