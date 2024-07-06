using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Compositions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace Iwan.Client.Blazor.Services.Compositions
{
    [Injected(ServiceLifetime.Scoped, typeof(ICompositionService))]
    public class CompositionService : ICompositionService
    {
        protected readonly HttpClient _client;

        public CompositionService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<CompositionDto> AddCompositionAsync(AddCompositionDto composition, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<CompositionDto, ApiErrorResponse>(
                Routes.Api.Admin.Compositions.BASE, composition, cancellationToken);
        }

        public async Task<CompositionImageDto> ChangeCompositionImageAsync(ChangeCompositionImageDto compositionImage, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<CompositionImageDto, ApiErrorResponse>(
                Routes.Api.Admin.Compositions.EditImage, compositionImage, cancellationToken);
        }

        public async Task DeleteCompositionAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = Routes.Api.Admin.Compositions.Delete.ReplaceRouteParameters(id);
            await _client.DeleteOrThrowAsync<ApiErrorResponse>(url, cancellationToken);
        }

        public async Task<CompositionDto> EditCompositionAsync(EditCompositionDto composition, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<CompositionDto, ApiErrorResponse>(
                Routes.Api.Admin.Compositions.BASE, composition, cancellationToken);
        }

        public async Task<PagedDto<CompositionDto>> GetCompositionsAsync(GetCompositionsOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareGetRequest(options, Routes.Api.Admin.Compositions.BASE);
            return await _client.SendAndReturnOrThrowAsync<PagedDto<CompositionDto>, ApiErrorResponse>(request, cancellationToken);
        }

        public async Task<CompositionDto> GetCompositionByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<CompositionDto, ApiErrorResponse>(
                Routes.Api.Admin.Compositions.GetComposition.ReplaceRouteParameters(id), cancellationToken);
        }

        #region Helpers

        private static HttpRequestMessage PrepareGetRequest(GetCompositionsOptions options, string url)
        {
            return PrepareGetRequest(options.ToQueryStringParameters(), url);
        }

        private static HttpRequestMessage PrepareGetRequest(string queryParameters, string url)
        {
            if (!queryParameters.IsNullOrEmptyOrWhiteSpaceSafe()) url += $"?{queryParameters}";

            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        #endregion
    }
}
