namespace Iwan.Server.Domain.Settings
{
    public class CategoriesImagesSettings : BaseEntity
    {
        public int MediumImageWidth { get; set; } = 800;
        public int MediumImageHeight { get; set; } = 800;
        public int MobileImageWidth { get; set; } = 200;
        public int MobileImageHeight { get; set; } = 200;
    }
}
