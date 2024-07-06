using Iwan.Shared.Dtos.Vendors;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Products
{
    public class ProductDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("arabicName")]
        public string ArabicName { get; set; }

        [JsonPropertyName("arabicDescription")]
        public string ArabicDescription { get; set; }

        [JsonPropertyName("dimensionsInArabic")]
        public string DimensionsInArabic { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("englishDescription")]
        public string EnglishDescription { get; set; }

        [JsonPropertyName("dimensionsInEnglish")]
        public string DimensionsInEnglish { get; set; }

        [JsonPropertyName("makerArabicName")]
        public string MakerArabicName { get; set; }

        [JsonPropertyName("makerEnglishName")]
        public string MakerEnglishName { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stockQuantity")]
        public int StockQuantity { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("colorCode")]
        public string ColorCode { get; set; }

        [JsonPropertyName("colorTypeId")]
        public int ColorTypeId { get; set; }

        [JsonPropertyName("isVisible")]
        public bool IsVisible { get; set; }

        [JsonPropertyName("hasMainImage")]
        public bool HasMainImage { get; set; }

        [JsonPropertyName("requiresResizing")]
        public bool RequiresResizing { get; set; }

        [JsonPropertyName("vendor")]
        public VendorDto Vendor { get; set; }

        [JsonPropertyName("mainImage")]
        public ProductMainImageDto MainImage { get; set; }

        [JsonPropertyName("images")]
        public IList<ProductImageDto> Images { get; set; }

        [JsonPropertyName("productCategories")]
        public IList<ProductCategoryDto> ProductCategories { get; set; }

        [JsonPropertyName("states")]
        public IList<ProductStateDto> States { get; set; }
    }
}
