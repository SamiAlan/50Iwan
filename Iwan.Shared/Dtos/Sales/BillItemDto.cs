using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Sales
{
    public class BillItemDto : TimedDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
    }
}
