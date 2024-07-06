using Iwan.Server.Infrastructure;
using Iwan.Server.Models;
using Iwan.Server.Models.Products;
using Iwan.Server.Options;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Options.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Products
{
    public interface IQueryProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(GetAllProductsOptions options, CancellationToken cancellationToken);
        
        Task<PagedDto<ProductDto>> GetProductsAsync(AdminGetProductsOptions options, CancellationToken cancellationToken);

        Task<PagedViewModel<ProductViewModel>> GetProductsAsync(GetProductsPageOptions options, CancellationToken cancellationToken);

        Task<IEnumerable<ProductStateDto>> GetProductStatesAsync(string productId, CancellationToken cancellationToken);

        Task<ProductStateDto> GetProductStateDetailsAsync(string productId, CancellationToken cancellationToken);
        
        Task<IEnumerable<ProductCategoryDto>> GetProductCategoriesAsync(string productId, CancellationToken cancellationToken);
        
        Task<IEnumerable<ProductImageDto>> GetProductImagesDetailsAsync(string id, CancellationToken cancellationToken);
        
        Task<ProductCategoryDto> GetProductCategoryDetailsAsync(string productCategoryId, CancellationToken cancellationToken);
        
        Task<ProductDto> GetProductDetailsAsync(string productId, bool includeImages = true, bool includeCategories = true, CancellationToken cancellationToken = default);

        Task<ProductImageDto> GetProductImageDetailsAsync(string productImageId, bool includeImages = true, CancellationToken cancellationToken = default);
        
        Task<ProductMainImageDto> GetProductMainImageDetailsAsync(string productMainImageId, CancellationToken cancellationToken);
        
        Task<ProductViewModel> GetProductDetailsViewModelAsync(string id, string categoryId, string subCategoryId, CancellationToken cancellationToken);
        
        Task<List<ProductImageViewModel>> GetProductImagesViewModelsAsync(string id, string categoryId, string subCategoryId, CancellationToken cancellationToken);
        
        Task<List<ProductStateViewModel>> GetProductStatesViewModelsAsync(string id, CancellationToken cancellationToken);
        
        Task<List<SimilarProductViewModel>> GetSimilarProductsViewModelsAsync(string id, CancellationToken cancellationToken);
        
        Task<PagedViewModel<ProductViewModel>> SearchProductsAsync(GetSearchProductsOptions options, CancellationToken cancellationToken);
    }
}
