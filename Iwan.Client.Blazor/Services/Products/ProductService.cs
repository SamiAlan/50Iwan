using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Files;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Options.Products;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Products
{
    [Injected(ServiceLifetime.Scoped, typeof(IProductService))]
    public class ProductService : IProductService
    {
        protected HttpClient _client;

        public ProductService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<PagedDto<ProductDto>> GetProductsAsync(AdminGetProductsOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareGetRequest(options, Routes.Api.Admin.Products.BASE);

            return await _client.SendAndReturnOrThrowAsync<PagedDto<ProductDto>, ApiErrorResponse>
                (request, cancellationToken);
        }

        public async Task<ProductDto> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<ProductDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.GetProduct.ReplaceRouteParameters(productId), cancellationToken);
        }

        public async Task<IEnumerable<ProductStateDto>> GetProductStatesAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<List<ProductStateDto>, ApiErrorResponse>
                (Routes.Api.Admin.Products.GetStates.ReplaceRouteParameters(productId), cancellationToken);
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategoriesAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<List<ProductCategoryDto>, ApiErrorResponse>
                (Routes.Api.Admin.Products.GetProductCategories.ReplaceRouteParameters(productId), cancellationToken);
        }

        public async Task<IEnumerable<ProductImageDto>> GetProductImagesAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<List<ProductImageDto>, ApiErrorResponse>
                (Routes.Api.Admin.Products.GetImages.ReplaceRouteParameters(productId), cancellationToken);
        }

        public async Task<ProductDto> AddProductAsync(AddProductDto product, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<ProductDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.BASE, product, cancellationToken);
        }

        public async Task<ProductDto> AddProductViaRarFileAsync(Stream fileSteam, string fileName, Action<long, double> onProgress = null, CancellationToken cancellationToken = default)
        {
            var request = PrepareAddViaRarRequest(fileSteam, Routes.Api.Admin.Products.AddViaRarFile, fileName, onProgress);

            return await _client.SendAndReturnOrThrowAsync<ProductDto, ApiErrorResponse>(request, cancellationToken);
        }

        public async Task<ProductStateDto> AddProductStateAsync(AddProductStateDto state, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<ProductStateDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.BASE_STATES, state, cancellationToken);
        }

        public async Task<ProductCategoryDto> AddProductCategoryAsync(AddProductCategoryDto productCategory, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<ProductCategoryDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.BASE_CATEGORIES, productCategory, cancellationToken);
        }

        public async Task<ProductImageDto> AddProductImageAsync(AddProductImageDto productImage, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<ProductImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.BASE_IMAGES, productImage, cancellationToken);
        }

        public async Task<ProductDto> EditProductAsync(EditProductDto product, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<ProductDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.BASE, product, cancellationToken);
        }

        public async Task<ProductMainImageDto> ChangeProductMainImageAsync(ChangeProductMainImageDto productImage, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<ProductMainImageDto, ApiErrorResponse>
                (Routes.Api.Admin.Products.ChangeMainImage, productImage, cancellationToken);
        }

        public async Task DeleteProductAsync(string productId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.DeleteProduct.ReplaceRouteParameters(productId), cancellationToken);
        }

        public async Task DeleteProductCategoryAsync(string productCategoryId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.DeleteProductCategory.ReplaceRouteParameters(productCategoryId), cancellationToken);
        }

        public async Task DeleteProductImageAsync(string productImageId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.DeleteProductImage.ReplaceRouteParameters(productImageId), cancellationToken);
        }

        public async Task DeleteProductStateAsync(string productStateId, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.DeleteState.ReplaceRouteParameters(productStateId), cancellationToken);
        }

        public async Task ResizeProductImagesAsync(string productId, CancellationToken cancellationToken = default)
        {
            await _client.PutOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.ResizeImage.ReplaceRouteParameters(productId), new object(), cancellationToken);
        }

        public async Task WatermarkProductImagesAsync(string productId, CancellationToken cancellationToken = default)
        {
            await _client.PutOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.WatermarkImage.ReplaceRouteParameters(productId), new object(), cancellationToken);
        }

        public async Task UnWatermarkProductImagesAsync(string productId, CancellationToken cancellationToken = default)
        {
            await _client.PutOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.UnWatermarkImage.ReplaceRouteParameters(productId), new object(), cancellationToken);
        }
        
        public async Task FlipVisibilityAsync(string productId, CancellationToken cancellationToken = default)
        {
            await _client.PutOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Products.FlipVisibility.ReplaceRouteParameters(productId), new object(), cancellationToken);
        }

        #region Helpers

        protected static HttpRequestMessage PrepareGetRequest(AdminGetProductsOptions options, string url)
        {
            var queryParameters = options.ToQueryStringParameters();

            if (!queryParameters.IsNullOrEmptyOrWhiteSpaceSafe()) url += $"?{queryParameters}";

            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        private static HttpRequestMessage PrepareAddViaRarRequest(Stream fileStream, string url, string fileName, Action<long, double> onProgress = null)
        {
            var content = new ProgressiveStreamContent(fileStream, 102400, onProgress);
            content.Headers.ContentType = new MediaTypeHeaderValue(Path.GetExtension(fileName).ToMimeType());
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new MultipartFormDataContent
                {
                    { content, "rarFile", fileName }
                }
            };

            return request;
        }

        #endregion
    }
}
