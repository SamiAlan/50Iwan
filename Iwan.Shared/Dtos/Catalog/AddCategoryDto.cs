using Iwan.Shared.Constants;

namespace Iwan.Shared.Dtos.Catalog
{
    public class AddCategoryDto
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public bool IsVisible { get; set; } = true;
        public string ParentCategoryId { get; set; }
        public string ColorCode { get; set; }
        public int ColorTypeId { get; set; } = (int)ColorType.NoChange;
        public AddCategoryImageDto Image { get; set; }

        public ColorType ColorType
        {
            get => (ColorType)ColorTypeId;
            set => ColorTypeId = (int)value;
        }
    }
}
