using Iwan.Server.Domain.Catelog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Catalog
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistAndAttachedSubCategoriesAsync(IList<string> subCategoriesIds, CancellationToken cancellationToken)
        {
            return (await _context.Categories.CountAsync(c => subCategoriesIds.Contains(c.Id) && c.IsSubCategory &&
                c.ParentCategoryId != null, cancellationToken)).Equals(subCategoriesIds.Count);
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(string parentCategoryId, CancellationToken cancellationToken = default)
        {
            return await Table.Where(c => c.ParentCategoryId == parentCategoryId)
                .ToListAsync(cancellationToken);
        }
    }
}
