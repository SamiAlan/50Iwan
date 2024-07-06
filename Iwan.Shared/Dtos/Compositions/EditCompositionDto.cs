using Iwan.Shared.Constants;

namespace Iwan.Shared.Dtos.Compositions
{
    public class EditCompositionDto
    {
        public string Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string ArabicDescription { get; set; }
        public string EnglishDescription { get; set; }
        public bool IsVisible { get; set; }
        public string ColorCode { get; set; }
        public int ColorTypeId { get; set; }

        public ColorType ColorType
        {
            get => (ColorType)ColorTypeId;
            set => ColorTypeId = (int)value;
        }
    }
}
