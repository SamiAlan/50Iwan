using Iwan.Shared.Dtos.Pages;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Pages
{
    public interface IPagesService
    {
        Task<ProductDetailsPageContentDto> GetProductDetailsPageContentAsync(CancellationToken cancellationToken = default);
        Task<HomePageContentDto> GetHomePageContentAsync(CancellationToken cancellationToken = default);
        Task<ColorDto> AddColorAsync(AddColorDto color, CancellationToken cancellationToken = default);
        Task<HeaderSectionDto> EditSectionAsync(EditHeaderSectionDto section, CancellationToken cancellationToken = default);
        Task<ContactUsSectionDto> EditSectionAsync(EditContactUsSectionDto section, CancellationToken cancellationToken = default);
        Task<AboutUsSectionDto> EditSectionAsync(EditAboutUsSectionDto section, CancellationToken cancellationToken = default);
        Task<ServicesSectionDto> EditSectionAsync(EditServicesSectionDto section, CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionDto> EditSectionAsync(EditInteriorDesignSectionDto section, CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionImageDto> ChangeMainImageAsync(ChangeInteriorDesignSectionMainImageDto image, CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionImageDto> ChangeMobileImageAsync(ChangeInteriorDesignSectionMobileImageDto image, CancellationToken cancellationToken = default);
        Task<AboutUsSectionImageDto> AddImageAsync(AddAboutUsSectionImageDto image, CancellationToken cancellationToken = default);
        Task DeleteAboutUsImageAsync(string aboutUsImageId, CancellationToken cancellationToken = default);
        Task DeleteColorAsync(string colorId, CancellationToken cancellationToken = default);
    }
}
