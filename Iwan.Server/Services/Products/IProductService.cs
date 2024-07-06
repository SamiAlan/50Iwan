using Iwan.Server.Domain.Products;
using Iwan.Shared.Dtos.Products;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Products
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(AddProductDto product, CancellationToken cancellationToken = default);
        
        Task<ProductImage> AddProductImageAsync(string productId, AddProductImageDto imageToAdd, CancellationToken cancellationToken = default);

        Task<ProductState> AddProductStateAsync(AddProductStateDto state, CancellationToken cancellationToken = default); 

        Task<Product> EditProductAsync(EditProductDto editedProduct, CancellationToken cancellationToken = default);
        
        Task<ProductMainImage> ChangeProductMainImageAsync(ChangeProductMainImageDto editedImage, CancellationToken cancellationToken = default);

        Task<ProductCategory> AddProductToCategoryAsync(string productId, string categoryId, CancellationToken cancellationToken = default);

        Task<(string, string)> RemoveProductCategoryAsync(string productCategoryId, CancellationToken cancellationToken = default);

        Task DeleteProductAsync(string productId, CancellationToken cancellationToken = default);

        Task DeleteProductImageAsync(string productImageId, CancellationToken cancellationToken);

        Task DeleteProductStateAsync(string stateId, CancellationToken cancellationToken = default);

        Task<ProductMainImage> ChangeProductMainImageAsync(string productId, string backgroundlessTempImageId, CancellationToken cancellationToken = default);
    }
}
