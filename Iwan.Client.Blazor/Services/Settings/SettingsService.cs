using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Settings
{
    [Injected(ServiceLifetime.Scoped, typeof(ISettingsService))]
    public class SettingsService : ISettingsService
    {
        protected readonly HttpClient _client;

        public SettingsService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<ImagesSettingsDto> GetImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<ImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.Images, cancellationToken);
        }

        public async Task<TempImagesSettingsDto> GetTempImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<TempImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.TEMPIMAGES_BASE, cancellationToken);
        }

        public async Task<WatermarkSettingsDto> GetWatermarkSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<WatermarkSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.WATERMARK_BASE, cancellationToken);
        }

        public async Task<CategoriesImagesSettingsDto> UpdateSettingsAsync(CategoriesImagesSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<CategoriesImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.Images_Categories, settings, cancellationToken);
        }

        public async Task<CompositionsImagesSettingsDto> UpdateSettingsAsync(CompositionsImagesSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<CompositionsImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.Images_Compositions, settings, cancellationToken);
        }

        public async Task<ProductsImagesSettingsDto> UpdateSettingsAsync(ProductsImagesSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<ProductsImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.Images_Products, settings, cancellationToken);
        }

        public async Task<SlidersImagesSettingsDto> UpdateSettingsAsync(SlidersImagesSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<SlidersImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.Images_Slider_Images, settings, cancellationToken);
        }

        public async Task<AboutUsSectionImagesSettingsDto> UpdateSettingsAsync(AboutUsSectionImagesSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<AboutUsSectionImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.Images_AboutUsSection, settings, cancellationToken);
        }

        public async Task<TempImagesSettingsDto> UpdateSettingsAsync(TempImagesSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<TempImagesSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.TEMPIMAGES_BASE, settings, cancellationToken);
        }

        public async Task<WatermarkSettingsDto> UpdateSettingsAsync(EditWatermarkSettingsDto settings, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<WatermarkSettingsDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.WATERMARK_BASE, settings, cancellationToken);
        }

        public async Task<WatermarkImageDto> ChangeWatermarkImageAsync(ChangeWatermarkImageDto newImage, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<WatermarkImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Settings.ChangeWatermarkImage, newImage, cancellationToken);
        }
    }
}
