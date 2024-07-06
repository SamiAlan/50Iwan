using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Dashboard
{
    public class ProductsReportLineDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("notVisibleCount")]
        public int NotVisibleCount { get; set; }
    }
}
