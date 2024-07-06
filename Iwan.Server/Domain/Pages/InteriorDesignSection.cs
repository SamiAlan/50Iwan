using System.Collections.Generic;

namespace Iwan.Server.Domain.Pages
{
    public class InteriorDesignSection : BaseEntity
    {
        public string ArabicText { get; set; }
        public string EnglishText { get; set; }
        public string Url { get; set; }
        public virtual InteriorDesignSectionImage Image { get; set; }
    }
}
