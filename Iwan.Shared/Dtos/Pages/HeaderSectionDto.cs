using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class HeaderSectionDto
    {
        [JsonPropertyName("arabicTitle")]
        public string ArabicTitle { get; set; }

        [JsonPropertyName("arabicSubtitle1")]
        public string ArabicSubtitle1 { get; set; }

        [JsonPropertyName("arabicSubtitle2")]
        public string ArabicSubtitle2 { get; set; }

        [JsonPropertyName("englishTitle")]
        public string EnglishTitle { get; set; }

        [JsonPropertyName("englishSubtitle1")]
        public string EnglishSubtitle1 { get; set; }

        [JsonPropertyName("englishSubtitle2")]
        public string EnglishSubtitle2 { get; set; }
    }
}
