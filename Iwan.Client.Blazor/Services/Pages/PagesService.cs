using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Pages
{
    [Injected(ServiceLifetime.Scoped, typeof(IPagesService))]
    public class PagesService : IPagesService
    {
        protected readonly HttpClient _client;

        public PagesService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<ColorDto> AddColorAsync(AddColorDto color, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<ColorDto, ApiErrorResponse>(Routes.Api.Admin.Pages.BASE_COLORS,
                color, cancellationToken);
        }

        public async Task<AboutUsSectionImageDto> AddImageAsync(AddAboutUsSectionImageDto image, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<AboutUsSectionImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_ABOUTUS_IMAGES, image, cancellationToken);
        }

        public async Task<InteriorDesignSectionImageDto> ChangeMainImageAsync(ChangeInteriorDesignSectionMainImageDto image, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<InteriorDesignSectionImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.ChangeInteriorDesignMainImage, image, cancellationToken);
        }

        public async Task<InteriorDesignSectionImageDto> ChangeMobileImageAsync(ChangeInteriorDesignSectionMobileImageDto image, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<InteriorDesignSectionImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.ChangeInteriorDesignMobileImage, image, cancellationToken);
        }

        public async Task DeleteAboutUsImageAsync(string aboutUsImageId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Pages.DeleteAboutUsImage.ReplaceRouteParameters(aboutUsImageId), cancellationToken);
        }

        public async Task DeleteColorAsync(string colorId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Pages.DeleteColor.ReplaceRouteParameters(colorId), cancellationToken);
        }

        public async Task<HeaderSectionDto> EditSectionAsync(EditHeaderSectionDto section, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<HeaderSectionDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_HEADER, section, cancellationToken);
        }

        public async Task<ContactUsSectionDto> EditSectionAsync(EditContactUsSectionDto section, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<ContactUsSectionDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_CONTACTUS, section, cancellationToken);
        }

        public async Task<AboutUsSectionDto> EditSectionAsync(EditAboutUsSectionDto section, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<AboutUsSectionDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_ABOUTUS, section, cancellationToken);
        }

        public async Task<ServicesSectionDto> EditSectionAsync(EditServicesSectionDto section, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<ServicesSectionDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_SERVICES, section, cancellationToken);
        }

        public async Task<InteriorDesignSectionDto> EditSectionAsync(EditInteriorDesignSectionDto section, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<InteriorDesignSectionDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_INTERIOR_DESIGN, section, cancellationToken);
        }

        public async Task<HomePageContentDto> GetHomePageContentAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<HomePageContentDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_HOME, cancellationToken);
        }

        public async Task<ProductDetailsPageContentDto> GetProductDetailsPageContentAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<ProductDetailsPageContentDto, ApiErrorResponse>
                (Routes.Api.Admin.Pages.BASE_PRODUCT_DETAILS, cancellationToken);
        }
    }
}
