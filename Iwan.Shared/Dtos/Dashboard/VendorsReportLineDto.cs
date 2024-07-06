using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Dashboard
{
    public class VendorsReportLineDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("topVendors")]
        public List<ReportLineVendorDto> TopVendors { get; set; } = new();
    }

    public class ReportLineVendorDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("relatedProductsCount")]
        public int RelatedProductsCount { get; set; }

        [JsonPropertyName("benefitPercent")]
        public decimal BenefitPercent { get; set; }
    }
}
