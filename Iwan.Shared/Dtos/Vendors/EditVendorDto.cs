namespace Iwan.Shared.Dtos.Vendors
{
    public class EditVendorDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool ShowOwnProducts { get; set; }
        public decimal BenefitPercent { get; set; }
    }
}
