namespace Iwan.Shared.Options.Sales
{
    public class GetBillsOptions : PagedOptions
    {
        public int? Number { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}
