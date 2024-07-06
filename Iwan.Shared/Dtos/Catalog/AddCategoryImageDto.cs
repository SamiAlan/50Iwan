using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Catalog
{
    public class AddCategoryImageDto
    {
        public AddImageDto Image { get; set; }

        public AddCategoryImageDto() { }
        public AddCategoryImageDto(AddImageDto image) => Image = image;
    }
}
