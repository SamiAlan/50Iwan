using Iwan.Server.Domain.Catelog;
using Iwan.Server.Domain.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<IEnumerable<ProductCategory>> GetByProductIdAsync(string productId, bool includeCategory = true, CancellationToken cancellationToken = default);
        Task<bool> ProductBelongToCategoryAsync(string productId, string categoryId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetCategoriesOfProductAsync(string productId, bool havingOnlyPassedProduct, CancellationToken cancellationToken);
    }
}
