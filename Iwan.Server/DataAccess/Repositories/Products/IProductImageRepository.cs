using Iwan.Server.Domain.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<IEnumerable<ProductImage>> GetImagesWithImageModelsAsync(string productId, CancellationToken cancellationToken = default);
        Task<ProductImage> GetImageWithImageModelsAsync(string productImageId, CancellationToken cancellationToken = default);
    }
}
