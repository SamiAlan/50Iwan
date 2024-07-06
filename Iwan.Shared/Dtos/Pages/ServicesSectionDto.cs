using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class ServicesSectionDto
    {
        [JsonPropertyName("service1ArabicTitle")]
        public string Service1ArabicTitle { get; set; }

        [JsonPropertyName("service1ArabicText")]
        public string Service1ArabicText { get; set; }

        [JsonPropertyName("service2ArabicTitle")]
        public string Service2ArabicTitle { get; set; }

        [JsonPropertyName("service2ArabicText")]
        public string Service2ArabicText { get; set; }

        [JsonPropertyName("service1EnglishTitle")]
        public string Service1EnglishTitle { get; set; }

        [JsonPropertyName("service1EnglishText")]
        public string Service1EnglishText { get; set; }

        [JsonPropertyName("service2EnglishTitle")]
        public string Service2EnglishTitle { get; set; }

        [JsonPropertyName("service2EnglishText")]
        public string Service2EnglishText { get; set; }
    }
}
