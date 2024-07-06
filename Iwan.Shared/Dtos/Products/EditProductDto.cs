using Iwan.Shared.Constants;

namespace Iwan.Shared.Dtos.Products
{
    public class EditProductDto
    {
        public string Id { get; set; }
        public string ArabicName { get; set; }
        public string ArabicDescription { get; set; }
        public string DimensionsInArabic { get; set; }
        public string EnglishName { get; set; }
        public string EnglishDescription { get; set; }
        public string DimensionsInEnglish { get; set; }
        public string MakerArabicName { get; set; }
        public string MakerEnglishName { get; set; }
        public int Age { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ColorCode { get; set; }
        public int ColorTypeId { get; set; }
        public bool IsVisible { get; set; }
        public string VendorId { get; set; }

        public ColorType ColorType
        {
            get => (ColorType)ColorTypeId;
            set => ColorTypeId = (int)value;
        }
    }
}
