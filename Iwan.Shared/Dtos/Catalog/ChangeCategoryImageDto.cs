using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Catalog
{
    public class ChangeCategoryImageDto
    {
        public string CategoryId { get; set; }
        public EditImageDto Image { get; set; }
    }
}
