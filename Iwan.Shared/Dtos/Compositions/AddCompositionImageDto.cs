using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Compositions
{
    public class AddCompositionImageDto
    {
        public AddImageDto Image { get; set; }

        public AddCompositionImageDto() { }
        public AddCompositionImageDto(AddImageDto image) => Image = image;
    }
}
