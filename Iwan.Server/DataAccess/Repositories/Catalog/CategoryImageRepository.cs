using Iwan.Server.Domain.Catelog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Catalog
{
    public class CategoryImageRepository : Repository<CategoryImage>, ICategoryImageRepository
    {
        public CategoryImageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<CategoryImage> GetByCategoryIncludingImagesAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.MobileImage)
                .SingleOrDefaultAsync(i => i.CategoryId == categoryId, cancellationToken);
        }

        public async Task<IDictionary<string, CategoryImage>> GetCategoriesImagesGroupedByIdAsync(IEnumerable<string> categoriesIds, CancellationToken cancellationToken = default)
        {
            return await Table.Include(i => i.OriginalImage)
                .Include(i => i.MediumImage)
                .Include(i => i.MobileImage)
                .Where(i => categoriesIds.Contains(i.CategoryId))
                .ToDictionaryAsync(i => i.CategoryId, i => i, cancellationToken);
        }
    }
}
