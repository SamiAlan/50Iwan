namespace Iwan.Server.Models.Products
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public string Dimensions { get; set; }
        public int Number { get; set; }
        public int Age { get; set; }
        public string Maker { get; set; }

        public ProductMainImageViewModel Image { get; set; }
    }
}
