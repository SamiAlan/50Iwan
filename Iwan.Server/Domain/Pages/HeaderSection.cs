namespace Iwan.Server.Domain.Pages
{
    public class HeaderSection : BaseEntity
    {
        public string ArabicTitle { get; set; }
        public string ArabicSubtitle1 { get; set; }
        public string ArabicSubtitle2 { get; set; }

        public string EnglishTitle { get; set; }
        public string EnglishSubtitle1 { get; set; }
        public string EnglishSubtitle2 { get; set; }
    }
}
