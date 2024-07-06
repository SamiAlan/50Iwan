using Iwan.Server.Infrastructure;
using Iwan.Server.Models.Catalog;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Options.Catalog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Catalog
{
    public interface IQueryCategoryService
    {   
        Task<IEnumerable<CategoryViewModel>> GetParentCategoriesForPublicAsync(CancellationToken cancellationToken = default);

        Task<ServiceResponse<IEnumerable<CategoryViewModel>>> GetSubCategoriesForPublicAsync(string parentCategoryId, CancellationToken cancellationToken = default);

        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(GetAllCategoriesOptions options, CancellationToken cancellationToken = default);

        Task<CategoryDto> GetCategoryDetailsAsync(string categoryId, bool includeImages = false, CancellationToken cancellationToken = default);

        Task<PagedDto<CategoryDto>> GetCategoriesAsync(GetCategoriesOptions options, CancellationToken cancellationToken = default);

        Task<string> GetLocalizedCategoryNameAsync(string categoryId, CancellationToken cancellationToken = default);
    }
}
