using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Options.Catalog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Catalog
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddCategoryAsync(AddCategoryDto category, CancellationToken cancellationToken = default);
        Task<CategoryDto> EditCategoryAsync(EditCategoryDto category, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(GetAllCategoriesOptions options, CancellationToken cancellationToken = default);
        Task<PagedDto<CategoryDto>> GetCategoriesAsync(GetCategoriesOptions options, CancellationToken cancellationToken = default);
        Task<CategoryDto> GetCategoryByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CategoryImageDto> ChangeCategoryImageAsync(ChangeCategoryImageDto categoryImage, CancellationToken cancellationToken = default);
        Task DeleteCategoryAsync(string id, CancellationToken cancellationToken = default);
        Task FlipVisibilityAsync(string id, CancellationToken cancellationToken = default);
    }
}
