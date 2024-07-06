using Iwan.Server.Domain.Catelog;
using Iwan.Server.Domain.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Products
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductCategory>> GetByProductIdAsync(string productId, bool includeCategory = true, CancellationToken cancellationToken = default)
        {
            if (!includeCategory)
                return await Table.Where(pc => pc.ProductId == productId).ToListAsync(cancellationToken);

            return await Table.Include(pc => pc.Category).Where(pc => pc.ProductId == productId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetCategoriesOfProductAsync(string productId, bool havingOnlyPassedProduct, CancellationToken cancellationToken)
        {
            if (havingOnlyPassedProduct)
            {
                return await _context.ProductCategories
                    .Where(pc => pc.ProductId == productId && 
                        !_context.ProductCategories.Any(innerPc => innerPc.CategoryId == pc.CategoryId && innerPc.ProductId != productId))
                    .Select(pc => pc.Category).ToListAsync(cancellationToken);
            }

            return await _context.ProductCategories.Where(pc => pc.ProductId == productId)
                    .Select(pc => pc.Category).ToListAsync(cancellationToken);
        }

        public async Task<bool> ProductBelongToCategoryAsync(string productId, string categoryId, CancellationToken cancellationToken = default)
        {
            return await Table.AnyAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId, cancellationToken);
        }
    }
}
