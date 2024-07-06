using Iwan.Server.Domain.Pages;
using Iwan.Shared.Dtos.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Pages
{
    public interface IPagesService
    {
        Task<Color> AddColorAsync(AddColorDto color, CancellationToken cancellationToken = default);
        Task<HeaderSection> EditSectionAsync(EditHeaderSectionDto section, CancellationToken cancellationToken = default);
        Task<ContactUsSection> EditSectionAsync(EditContactUsSectionDto section, CancellationToken cancellationToken = default);
        Task<AboutUsSection> EditSectionAsync(EditAboutUsSectionDto section, CancellationToken cancellationToken = default);
        Task<ServicesSection> EditSectionAsync(EditServicesSectionDto section, CancellationToken cancellationToken = default);
        Task<InteriorDesignSection> EditSectionAsync(EditInteriorDesignSectionDto section, CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionImage> ChangeMainImageAsync(ChangeInteriorDesignSectionMainImageDto image, CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionImage> ChangeMobileImageAsync(ChangeInteriorDesignSectionMobileImageDto image, CancellationToken cancellationToken = default);
        Task<AboutUsSectionImage> AddImageAsync(AddAboutUsSectionImageDto image, CancellationToken cancellationToken = default);
        Task DeleteColorAsync(string colorId, CancellationToken cancellationToken = default);
        Task DeleteAboutUsImageAsync(string aboutUsImageId, CancellationToken cancellationToken = default);
    }
}
