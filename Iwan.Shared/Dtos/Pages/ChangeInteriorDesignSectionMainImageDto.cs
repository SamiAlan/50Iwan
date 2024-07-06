using Iwan.Shared.Dtos.Media;

namespace Iwan.Shared.Dtos.Pages
{
    public class ChangeInteriorDesignSectionMainImageDto
    {
        public EditImageDto MainImage { get; set; }

        public ChangeInteriorDesignSectionMainImageDto() { }
        public ChangeInteriorDesignSectionMainImageDto(EditImageDto image) => MainImage = image;
    }

    public class ChangeInteriorDesignSectionMobileImageDto
    {
        public EditImageDto MobileImage { get; set; }

        public ChangeInteriorDesignSectionMobileImageDto() { }
        public ChangeInteriorDesignSectionMobileImageDto(EditImageDto image) => MobileImage = image;
    }
}
