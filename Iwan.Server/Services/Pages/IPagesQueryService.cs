using Iwan.Server.Domain.Pages;
using Iwan.Server.Models.Pages;
using Iwan.Server.Options;
using Iwan.Shared.Dtos.Pages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Pages
{
    public interface IPagesQueryService
    {
        Task<HomePageContentDto> GetHomePageContentDetailsAsync(CancellationToken cancellationToken = default);
        Task<ProductDetailsPageContentDto> GetProductDetailsContentPageAsync(CancellationToken cancellationToken = default);
        Task<ColorPickingSectionDto> GetColorPickingSectionDetailsAsync(CancellationToken cancellationToken = default);
        Task<HeaderSectionDto> GetHeaderSectionDetailsAsync(CancellationToken cancellationToken = default);
        Task<ContactUsSectionDto> GetContactUsDetailsAsync(CancellationToken cancellationToken = default);
        Task<AboutUsSectionDto> GetAboutUsSectionDetailsAsync(CancellationToken cancellationToken = default);
        Task<ProductsPageContentViewModel> GetProductsPageContentAsync(GetProductsPageOptions options, CancellationToken cancellationToken);
        Task<AboutUsSectionImageDto> GetAboutUsSectionImageDetailsAsync(string imageId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AboutUsSectionImageDto>> GetAboutUsSectionImagesDetailsAsync(CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionDto> GetInteriorDesignSectionDetailsAsync(CancellationToken cancellationToken = default);
        Task<InteriorDesignSectionImageDto> GetInteriorDesignSectionImageDetailsAsync(CancellationToken cancellationToken = default);
        Task<ServicesSectionDto> GetServicesSectionDetailsAsync(CancellationToken cancellationToken = default);
        Task<ProductDetailsPageContentViewModel> GetProductDetailsPageContentAsync(string productId, string categoryId, CancellationToken cancellationToken);
        Task<HeaderSection> GetHeaderSectionAsync(CancellationToken cancellationToken = default);
        Task<ColorPickingSection> GetColorPickingSectionAsync(CancellationToken cancellationToken = default);
        Task<ContactUsSection> GetContactUsSectionAsync(CancellationToken cancellationToken = default);
        Task<AboutUsSection> GetAboutUsSectionAsync(CancellationToken cancellationToken = default);
        Task<InteriorDesignSection> GetInteriorDesignSectionAsync(CancellationToken cancellationToken = default);
        Task<SearchPageContentViewModel> GetSearchPageContentAsync(GetSearchProductsOptions options, CancellationToken cancellationToken);
        Task<ServicesSection> GetServicesSectionAsync(CancellationToken cancellationToken = default);
    }
}
