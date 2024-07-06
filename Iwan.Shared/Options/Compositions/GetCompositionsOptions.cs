namespace Iwan.Shared.Options.Compositions
{
    public class GetCompositionsOptions : PagedOptions
    {
        public string Text { get; set; }
        public bool? OnlyVisible { get; set; }
        public bool IncludeImages { get; set; }
    }
}
