using Iwan.Server.Domain.Catelog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Catalog
{
    public interface ICategoryImageRepository : IRepository<CategoryImage>
    {
        Task<CategoryImage> GetByCategoryIncludingImagesAsync(string categoryId, CancellationToken cancellationToken = default);
        Task<IDictionary<string, CategoryImage>> GetCategoriesImagesGroupedByIdAsync(IEnumerable<string> categoriesIds, CancellationToken cancellationToken = default);
    }
}
