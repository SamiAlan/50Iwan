using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Options.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Products
{
    public interface IProductService
    {
        Task<PagedDto<ProductDto>> GetProductsAsync(AdminGetProductsOptions options, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductCategoryDto>> GetProductCategoriesAsync(string productId, CancellationToken cancellationToken = default);
        Task<ProductDto> GetProductByIdAsync(string productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductImageDto>> GetProductImagesAsync(string productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductStateDto>> GetProductStatesAsync(string productId, CancellationToken cancellationToken = default);
        Task<ProductDto> AddProductAsync(AddProductDto product, CancellationToken cancellationToken = default);
        Task<ProductDto> AddProductViaRarFileAsync(Stream fileSteam, string fileName, Action<long, double> onProgress = null, CancellationToken cancellationToken = default);
        Task<ProductStateDto> AddProductStateAsync(AddProductStateDto state, CancellationToken cancellationToken = default);
        Task<ProductImageDto> AddProductImageAsync(AddProductImageDto productImage, CancellationToken cancellationToken = default);
        Task<ProductCategoryDto> AddProductCategoryAsync(AddProductCategoryDto productCategory, CancellationToken cancellationToken = default);
        Task<ProductDto> EditProductAsync(EditProductDto product, CancellationToken cancellationToken = default);
        Task<ProductMainImageDto> ChangeProductMainImageAsync(ChangeProductMainImageDto productImage, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(string productId, CancellationToken cancellationToken = default);
        Task DeleteProductCategoryAsync(string productCategoryId, CancellationToken cancellationToken = default);
        Task DeleteProductStateAsync(string productStateId, CancellationToken cancellationToken = default);
        Task DeleteProductImageAsync(string productImageId, CancellationToken cancellationToken = default);
        Task ResizeProductImagesAsync(string productId, CancellationToken cancellationToken = default);
        Task WatermarkProductImagesAsync(string productId, CancellationToken cancellationToken = default);
        Task UnWatermarkProductImagesAsync(string productId, CancellationToken cancellationToken = default);
        Task FlipVisibilityAsync(string productId, CancellationToken cancellationToken = default);
    }
}
