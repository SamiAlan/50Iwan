using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.SliderImages;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.SliderImages
{
    [Injected(ServiceLifetime.Scoped, typeof(ISliderImageService))]
    public class SliderImageService : ISliderImageService
    {
        protected readonly HttpClient _client;
        
        public SliderImageService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<SliderImageDto> GetSliderImageAsync(string id, CancellationToken cancellation = default)
        {
            return await _client.GetAndReturnOrThrowAsync<SliderImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Sliders.GetSlider.ReplaceRouteParameters(id), cancellation);
        }

        public async Task AddSliderImageAsync(AddSliderImageDto sliderImage, CancellationToken cancellationToken = default)
        {
            await _client.PostAndReturnOrThrowAsync<SliderImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Sliders.BASE, sliderImage, cancellationToken);
        }

        public async Task DeleteSliderImageAsync(string id, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Sliders.Delete.ReplaceRouteParameters(id), cancellationToken);
        }

        public async Task EditSliderImageAsync(EditSliderImageDto sliderImage, CancellationToken cancellationToken = default)
        {
            await _client.PutAndReturnOrThrowAsync<SliderImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Sliders.BASE, sliderImage, cancellationToken);
        }

        public async Task<PagedDto<SliderImageDto>> GetSliderImagesAsync(GetSliderImagesOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareRequest(options, Routes.Api.Admin.Sliders.BASE);

            return await _client.SendAndReturnOrThrowAsync<PagedDto<SliderImageDto>, ApiErrorResponse>(request, cancellationToken);
        }

        #region Helpers

        private static HttpRequestMessage PrepareRequest(GetSliderImagesOptions options, string url)
        {
            var queryStrings = options.ToQueryStringParameters();

            if (!queryStrings.IsNullOrEmptyOrWhiteSpaceSafe()) url += $"?{queryStrings}";

            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        #endregion
    }
}
