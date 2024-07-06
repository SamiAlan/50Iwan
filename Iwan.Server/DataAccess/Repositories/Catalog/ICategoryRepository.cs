using Iwan.Server.Domain.Catelog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.DataAccess.Repositories.Catalog
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> ExistAndAttachedSubCategoriesAsync(IList<string> subCategoriesIds, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetSubCategoriesAsync(string parentCategoryId, CancellationToken cancellationToken = default);
    }
}
