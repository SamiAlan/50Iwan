using Iwan.Client.Blazor.Constants;
using Iwan.Shared.Dtos.Media;
using System.Net.Http;
using System.Threading.Tasks;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Extensions;
using System.IO;
using System.Threading;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Iwan.Client.Blazor.Infrastructure.Files;
using System;

namespace Iwan.Client.Blazor.Services.Media
{
    [Injected(ServiceLifetime.Scoped, typeof(IImageService))]
    public class ImageService : IImageService
    {
        protected readonly HttpClient _client;

        public ImageService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(HttpClientsNames.Base);
        }

        public async Task DeleteTempImageAsync(string tempImageId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>(Routes.Api.Admin.Images.DeleteTemp.ReplaceRouteParameters(tempImageId), cancellationToken);
        }

        public async Task<TempImageDto> UploadTempImageAsync(Stream fileSteam, string fileName, Action<long, double> onProgress = null,  CancellationToken cancellationToken = default)
        {
            var request = PrepareRequest(fileSteam, Routes.Api.Admin.Images.BASE_TEMP, fileName, onProgress);

            return await _client.SendAndReturnOrThrowAsync<TempImageDto, ApiErrorResponse>(request, cancellationToken);
        }

        #region Helpers

        private static HttpRequestMessage PrepareRequest(Stream fileStream, string url, string fileName, Action<long, double> onProgress = null)
        {
            var content = new ProgressiveStreamContent(fileStream, 40096, onProgress);
            content.Headers.ContentType = new MediaTypeHeaderValue(Path.GetExtension(fileName).ToMimeType());
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new MultipartFormDataContent
                {
                    { content, "image", fileName }
                }
            };

            return request;
        }

        #endregion
    }
}
