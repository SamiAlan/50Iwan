using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Sales
{
    public class BillDto : TimedDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customerPhone")]
        public string CustomerPhone { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("billItems")]
        public IList<BillItemDto> BillItems { get; set; }
    }
}
