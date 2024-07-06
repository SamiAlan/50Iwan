using Iwan.Server.Models.Catalog;
using System.Collections.Generic;

namespace Iwan.Server.Models.Pages
{
    public class HomePageContentViewModel : PageViewModel
    {
        public HeaderSectionViewModel HeadSection { get; set; }
        public List<SliderImageViewModel> SliderImages { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public InteriorDesignSectionViewModel InteriorDesignSection { get; set; }
        public ServicesSectionViewModel ServicesSection { get; set; }
        public AboutUsSectionViewModel AboutUsSection { get; set; }
        public ContactUsSectionViewModel ContactUsSection { get; set; }
    }
}
