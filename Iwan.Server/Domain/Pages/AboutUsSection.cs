using System.Collections.Generic;

namespace Iwan.Server.Domain.Pages
{
    public class AboutUsSection : BaseEntity
    {
        public string ArabicText { get; set; }
        public string EnglishText { get; set; }
        public virtual ICollection<AboutUsSectionImage> Images { get; set; }
    }
}
