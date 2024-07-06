using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Compositions
{
    public class ChangeCompositionImageDto
    {
        public string CompositionId { get; set; }
        public EditImageDto Image { get; set; }
    }
}
