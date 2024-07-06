using Iwan.Server.Domain.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public interface IProductMainImageRepository : IRepository<ProductMainImage>
    {
        Task<ProductMainImage> GetProductMainImageAsync(string productId, CancellationToken token = default);
        Task<ProductMainImage> GetAndIncludeImagesAsync(string productMainImageId, CancellationToken cancellationToken = default);
        Task<IDictionary<string, ProductMainImage>> GetProductsImagesGroupedByIdAsync(IEnumerable<string> enumerable, CancellationToken cancellationToken);
        Task<ProductMainImage> GetByProductIdAndIncludeImagesAsync(string productId, CancellationToken cancellationToken);
    }
}
