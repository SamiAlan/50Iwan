using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Dashboard
{
    public class DashboardDto
    {
        [JsonPropertyName("generalReportLine")]
        public GeneralReportLineDto GeneralReportLine { get; set; } = new();

        [JsonPropertyName("categoriesReportLine")]
        public CategoriesReportLineDto CategoriesReportLine { get; set; } = new();

        [JsonPropertyName("productsReportLine")]
        public ProductsReportLineDto ProductsReportLine { get; set; } = new();

        [JsonPropertyName("billsReportLine")]
        public BillsReportLineDto BillsReportLine { get; set; } = new();

        [JsonPropertyName("vendorsReportLine")]
        public VendorsReportLineDto VendorsReportLine { get; set; } = new();
    }
}
