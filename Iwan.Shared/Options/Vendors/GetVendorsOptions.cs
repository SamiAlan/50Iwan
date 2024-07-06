namespace Iwan.Shared.Options.Vendors
{
    public class GetVendorsOptions : PagedOptions
    {
        public string Name { get; set; }
        public bool? OnlyVendorsShowingTheirProducts { get; set; }
        public decimal? MinBenefitPercentage { get; set; }
        public decimal? MaxBenefitPercentage { get; set; }
    }
}
