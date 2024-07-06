using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Pages
{
    public class AddAboutUsSectionImageDto
    {
        public AddImageDto Image { get; set; }

        public AddAboutUsSectionImageDto() { }
        public AddAboutUsSectionImageDto(AddImageDto image) => Image = image;
    }
}
