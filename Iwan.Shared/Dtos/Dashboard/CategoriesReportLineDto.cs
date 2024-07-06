using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Dashboard
{
    public class CategoriesReportLineDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("parentCategoriesCount")]
        public int ParentCategoriesCount { get; set; }

        [JsonPropertyName("subCategoriesCount")]
        public int SubCategoriesCount{ get; set; }
    }
}
