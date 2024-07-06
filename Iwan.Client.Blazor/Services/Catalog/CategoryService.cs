using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Catalog;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Catalog
{
    [Injected(ServiceLifetime.Scoped, typeof(ICategoryService))]
    public class CategoryService : ICategoryService
    {
        protected readonly HttpClient _client;

        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(HttpClientsNames.Base);
        }
      
        public async Task<CategoryDto> AddCategoryAsync(AddCategoryDto category, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<CategoryDto, ApiErrorResponse>(
                Routes.Api.Admin.Categories.BASE, category, cancellationToken);
        }

        public async Task<CategoryImageDto> ChangeCategoryImageAsync(ChangeCategoryImageDto categoryImage, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<CategoryImageDto, ApiErrorResponse>(
                Routes.Api.Admin.Categories.ChangeImage, categoryImage, cancellationToken);
        }

        public async Task DeleteCategoryAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = Routes.Api.Admin.Categories.Delete.ReplaceRouteParameters(id);
            await _client.DeleteOrThrowAsync<ApiErrorResponse>(url, cancellationToken);
        }

        public async Task FlipVisibilityAsync(string id, CancellationToken cancellationToken = default)
        {
            var url = Routes.Api.Admin.Categories.FlipVisibility.ReplaceRouteParameters(id);
            await _client.PutOrThrowAsync<ApiErrorResponse>(url, new object(), cancellationToken);
        }

        public async Task<CategoryDto> EditCategoryAsync(EditCategoryDto category, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<CategoryDto, ApiErrorResponse>(
                Routes.Api.Admin.Categories.BASE, category, cancellationToken);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(GetAllCategoriesOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareGetRequest(options, Routes.Api.Admin.Categories.GetAll);
            return await _client.SendAndReturnOrThrowAsync<List<CategoryDto>, ApiErrorResponse>(request, cancellationToken);
        }

        public async Task<PagedDto<CategoryDto>> GetCategoriesAsync(GetCategoriesOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareGetRequest(options, Routes.Api.Admin.Categories.BASE);
            return await _client.SendAndReturnOrThrowAsync<PagedDto<CategoryDto>, ApiErrorResponse>(request, cancellationToken);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<CategoryDto, ApiErrorResponse>(
                Routes.Api.Admin.Categories.GetCategory.ReplaceRouteParameters(id), cancellationToken);
        }

        #region Helpers

        private static HttpRequestMessage PrepareGetRequest(GetAllCategoriesOptions options, string url)
        {
            return PrepareGetRequest(options.ToQueryStringParameters(), url);
        }

        private static HttpRequestMessage PrepareGetRequest(GetCategoriesOptions options, string url)
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
