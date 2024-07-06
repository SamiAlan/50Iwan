using Iwan.Shared.Dtos.Common;
namespace Iwan.Shared.Dtos.Vendors
{
    public class AddVendorDto
    {
        public string Name { get; set; }
        public bool ShowOwnProducts { get; set; }
        public decimal BenefitPercent { get; set; }
        public AddAddressDto Address { get; set; }

        public AddVendorDto()
        {
            Address = new();
        }
    }
}
