using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Media
{
    public class ImageDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
