using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class InteriorDesignSectionDto
    {
        [JsonPropertyName("arabicText")]
        public string ArabicText { get; set; }

        [JsonPropertyName("englishText")]
        public string EnglishText { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("images")]
        public InteriorDesignSectionImageDto Image { get; set; }
    }
}
