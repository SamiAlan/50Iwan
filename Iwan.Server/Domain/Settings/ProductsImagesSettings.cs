namespace Iwan.Server.Domain.Settings
{
    public class ProductsImagesSettings : TimedEntity
    {
        public int MediumImageWidth { get; set; } = 800;
        public int MediumImageHeight { get; set; } = 800;
        public int SmallImageWidth { get; set; } = 50;
        public int SmallImageHeight { get; set; } = 50;
        public int MobileImageWidth { get; set; } = 200;
        public int MobileImageHeight { get; set; } = 200;
    }
}
