using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Pages
{
    public class ColorPickingSectionDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("colors")]
        public IList<ColorDto> Colors { get; set; } = new List<ColorDto>();
    }

    public class ColorDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("colorCode")]
        public string ColorCode { get; set; }
    }

    public class AddColorDto
    {
        public string ColorCode { get; set; }
    }
}
