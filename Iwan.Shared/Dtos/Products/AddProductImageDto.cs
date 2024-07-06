using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Products
{
    public class AddProductImageDto
    {
        public string ProductId { get; set; }
        public AddImageDto Image { get; set; }

        public AddProductImageDto() {}
        public AddProductImageDto(AddImageDto image) => Image = image;
        public AddProductImageDto(string productId, AddImageDto image) : this(image) => ProductId = productId;
    }
}
