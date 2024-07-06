using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Products
{
    public class ChangeProductMainImageDto
    {
        public string ProductId { get; set; }
        public EditImageDto Image { get; set; }
    }
}
