using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class AboutUsSectionDto
    {
        [JsonPropertyName("arabicText")]
        public string ArabicText { get; set; }

        [JsonPropertyName("englishText")]
        public string EnglishText { get; set; }

        [JsonPropertyName("images")]
        public List<AboutUsSectionImageDto> Images { get; set; }
    }
}
