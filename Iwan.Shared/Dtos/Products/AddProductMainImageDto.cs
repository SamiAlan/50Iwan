using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Products
{
    public class AddProductMainImageDto
    {
        public AddImageDto Image { get; set; }

        public AddProductMainImageDto() { }
        public AddProductMainImageDto(AddImageDto image) => Image = image;
    }
}
