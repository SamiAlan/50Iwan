using Iwan.Server.Domain.Catelog;
using Iwan.Shared.Dtos.Catalog;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Catalog
{
    public interface ICategoryService
    {
        Task<Category> AddCategoryAsync(AddCategoryDto categoryToAdd, CancellationToken cancellationToken = default);

        Task<Category> EditCategoryAsync(EditCategoryDto editedCategory, CancellationToken cancellationToken = default);

        Task DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default);

        Task<CategoryImage> ChangeCategoryImageAsync(string categoryId, string bacgroundlessCroppedTempImageId, CancellationToken cancllationToken = default);

        Task<CategoryImage> EditCategoryImageAsync(EditCategoryImageDto editedImage, CancellationToken cancellationToken = default);
    }
}
