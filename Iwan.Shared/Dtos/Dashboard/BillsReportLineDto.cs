using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Dashboard
{
    public class BillsReportLineDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("unPaiedBills")]
        public int UnPaiedBills { get; set; }

        [JsonPropertyName("totalAmountToBePaied")]
        public decimal TotalAmountToBePaied { get; set; }

        [JsonPropertyName("totalValue")]
        public decimal TotalValue { get; set; }
    }
}
