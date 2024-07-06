namespace Iwan.Server.Domain.Settings
{
    public class AboutUsSectionImagesSettings : BaseEntity
    {
        public int MediumImageWidth { get; set; } = 1000;
        public int MediumImageHeight { get; set; } = 1000;
        public int MobileImageWidth { get; set; } = 200;
        public int MobileImageHeight { get; set; } = 200;
    }
}
