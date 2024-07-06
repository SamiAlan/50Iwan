using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Server.Extensions;
using Iwan.Server.Infrastructure;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Models.Catalog;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwan.Server.Services.Accounts;

namespace Iwan.Server.Services.Catalog
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryCategoryService))]
    public class QueryCategoryService : IQueryCategoryService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAppImageHelper _appImageHelper;
        protected readonly ILoggedInUserProvider _loggedInUser;

        public QueryCategoryService(IUnitOfWork unitOfWork, IAppImageHelper appUrlHelper,
            ILoggedInUserProvider loggedInUser)
        {
            _unitOfWork = unitOfWork;
            _appImageHelper = appUrlHelper;
            _loggedInUser = loggedInUser;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(GetAllCategoriesOptions options, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.CategoriesRepository.Table;

            if (options.OnlyVisible.HasValue)
                query = query.Where(c => c.IsVisible == options.OnlyVisible.Value);

            if (options.OnlyHasRelatedProducts.HasValue)
                query = query.Where(c => c.HasRelatedProducts == options.OnlyHasRelatedProducts.Value);

            if (!options.UnderParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(c => c.ParentCategoryId == options.UnderParentCategoryId);

            if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(c => c.EnglishName.ToLower().Contains(options.Text.ToLower()) || c.ArabicName.ToLower().Contains(options.Text.ToLower()));

            if (options.Type.HasValue)
                query = options.Type.Value switch
                {
                    CategoryType.Parent => query.Where(c => !c.IsSubCategory),
                    CategoryType.SubCategory => query.Where(c => c.IsSubCategory),
                    _ => query
                };

            if (!options.IncludeImages)
                return query.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    IsVisible = c.IsVisible,
                    ArabicName = c.ArabicName,
                    EnglishName = c.EnglishName
                });

            var categories = await query.ToListAsync(cancellationToken);

            var categoriesIds = categories.Select(c => c.Id);

            var categoriesImagesByCategoryId = await _unitOfWork.CategoryImagesRepository.GetCategoriesImagesGroupedByIdAsync(categoriesIds, cancellationToken);

            var categoriesDtos = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var categoryImage = categoriesImagesByCategoryId[category.Id];

                categoriesDtos.Add(new CategoryDto
                {
                    Id = category.Id,
                    IsVisible = category.IsVisible,
                    ArabicName = category.ArabicName,
                    EnglishName = category.EnglishName,
                    Image = new CategoryImageDto
                    {
                        Id = categoryImage.Id,
                        OriginalImage = _appImageHelper.GenerateImageDto(categoryImage.OriginalImage),
                        MediumImage = _appImageHelper.GenerateImageDto(categoryImage.MediumImage),
                        MobileImage = _appImageHelper.GenerateImageDto(categoryImage.MobileImage)
                    }
                });
            }

            return categoriesDtos;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetParentCategoriesForPublicAsync(CancellationToken cancellationToken = default)
        {
            var categoriesModels = new List<CategoryViewModel>();

            var parentCategories = await _unitOfWork.CategoriesRepository.Table.Include(c => c.Image)
                .Where(c => c.IsVisible && !c.IsSubCategory).ToListAsync(cancellationToken);

            var parentCategoriesIds = parentCategories.Select(c => c.Id);

            var categoriesImages = await _unitOfWork.CategoryImagesRepository.GetCategoriesImagesGroupedByIdAsync(parentCategoriesIds, cancellationToken);

            foreach (var category in parentCategories)
            {
                var isCategoryColorSetAndValid = category.ColorType == ColorType.Custom && category.ColorCode.IsHtmlColor();
                var categoryColorCode = category.ColorCode;

                var categoryImage = categoriesImages[category.Id];

                var finalColor = string.Empty;
                var hasBackgroundColor = false;

                if (category.ColorType == ColorType.Custom)
                {
                    finalColor = category.ColorCode;
                    hasBackgroundColor = true;
                }
                else if (category.ColorType == ColorType.FromParent && isCategoryColorSetAndValid)
                {
                    finalColor = categoryColorCode;
                    hasBackgroundColor = true;
                }

                var croppedImage = categoryImage.OriginalImage;
                var mediumImage = categoryImage.MediumImage;
                var mobileImage = categoryImage.MobileImage;

                categoriesModels.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = _loggedInUser.IsCultureArabic ? category.ArabicName : category.EnglishName,
                    Image = new CategoryImageModel
                    {
                        HasBackgroundColor = hasBackgroundColor,
                        ColorCode = finalColor,
                        OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                        MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                        MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                    }
                });
            }

            return categoriesModels;
        }

        public async Task<ServiceResponse<IEnumerable<CategoryViewModel>>> GetSubCategoriesForPublicAsync(string parentCategoryId, CancellationToken cancellationToken = default)
        {
            var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(parentCategoryId, cancellationToken);

            if (parentCategory == null)
                return new NotFoundResponse<IEnumerable<CategoryViewModel>>(Responses.General.ResourceNotFound);

            var subCategories = await _unitOfWork.CategoriesRepository.Where(c => c.ParentCategoryId == parentCategoryId && c.IsVisible && c.HasRelatedProducts)
                .ToListAsync(cancellationToken);

            var isParentColorSetAndValid = parentCategory.ColorType == ColorType.Custom && parentCategory.ColorCode.IsHtmlColor();
            var parentCategoryColorCode = parentCategory.ColorCode;

            var subCategoriesIds = subCategories.Select(c => c.Id);

            var subCategoriesImages = await _unitOfWork.CategoryImagesRepository.GetCategoriesImagesGroupedByIdAsync(subCategoriesIds, cancellationToken);

            var subCategoriesViewModels = new List<CategoryViewModel>();

            foreach (var subCategory in subCategories)
            {
                var subCategoryImage = subCategoriesImages[subCategory.Id];
                var isSubCategoryColorSetAndValid = subCategory.ColorType == ColorType.Custom && subCategory.ColorCode.IsHtmlColor();

                var finalSubCategoryImageColor = string.Empty;
                var subcCategoryImageHasBackground = false;

                if (subCategory.ColorType == ColorType.Custom)
                {
                    finalSubCategoryImageColor = subCategory.ColorCode;
                    subcCategoryImageHasBackground = true;
                }
                else if (subCategory.ColorType == ColorType.FromParent)
                {
                    if (isSubCategoryColorSetAndValid)
                    {
                        finalSubCategoryImageColor = subCategory.ColorCode;
                        subcCategoryImageHasBackground = true;
                    }
                    else if (subCategory.ColorType == ColorType.FromParent && isParentColorSetAndValid)
                    {
                        finalSubCategoryImageColor = parentCategoryColorCode;
                        subcCategoryImageHasBackground = true;
                    }
                }

                var croppedImage = subCategoryImage.OriginalImage;
                var mediumImage = subCategoryImage.MediumImage;
                var mobileImage = subCategoryImage.MobileImage;

                subCategoriesViewModels.Add(new CategoryViewModel
                {
                    Id = subCategory.Id,
                    Name = _loggedInUser.IsCultureArabic ? subCategory.ArabicName : subCategory.EnglishName,
                    Image = new CategoryImageModel
                    {
                        HasBackgroundColor = subcCategoryImageHasBackground,
                        ColorCode = finalSubCategoryImageColor,
                        OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                        MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                        MobileImage = _appImageHelper.GenerateImageModel(mobileImage),
                    }
                });
            }

            return new ServiceResponse<IEnumerable<CategoryViewModel>>(subCategoriesViewModels);
        }

        public async Task<CategoryDto> GetCategoryDetailsAsync(string categoryId, bool includeImages = false, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);

            if (category == null)
                throw new NotFoundException(Responses.Categories.CategoryNotExist);

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                ArabicName = category.ArabicName,
                EnglishName = category.EnglishName,
                IsVisible = category.IsVisible,
                ColorCode = category.ColorCode,
                ColorTypeId = category.ColorTypeId,
                ParentCategoryId = category.ParentCategoryId
            };

            if (!includeImages)
                return categoryDto;

            var categoryImage = await _unitOfWork.CategoryImagesRepository.GetByCategoryIncludingImagesAsync(category.Id, cancellationToken);

            categoryDto.Image = new CategoryImageDto
            {
                Id = categoryImage.Id,
                OriginalImage = _appImageHelper.GenerateImageDto(categoryImage.OriginalImage),
                MediumImage = _appImageHelper.GenerateImageDto(categoryImage.MediumImage),
                MobileImage = _appImageHelper.GenerateImageDto(categoryImage.MobileImage)
            };

            return categoryDto;
        }

        public async Task<PagedDto<CategoryDto>> GetCategoriesAsync(GetCategoriesOptions options, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.CategoriesRepository.Table;

            if (options.OnlyVisible.HasValue)
                query = query.Where(c => c.IsVisible == options.OnlyVisible.Value);

            if (options.OnlyHasRelatedProducts.HasValue)
                query = query.Where(c => c.HasRelatedProducts == options.OnlyHasRelatedProducts.Value);

            if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(c => c.EnglishName.ToLower().Contains(options.Text.ToLower()) || c.ArabicName.ToLower().Contains(options.Text.ToLower()));

            if (!options.UnderParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(c => c.ParentCategoryId == options.UnderParentCategoryId);

            if (options.Type.HasValue)
                query = options.Type.Value switch
                {
                    CategoryType.Parent => query.Where(c => !c.IsSubCategory),
                    CategoryType.SubCategory => query.Where(c => c.IsSubCategory),
                    _ => query
                };

            var totalCount = await query.CountAsync(cancellationToken);

            query = query.Skip(options.CurrentPage * options.PageSize).Take(options.PageSize);

            var categories = await query.ToListAsync(cancellationToken);

            if (!options.IncludeImages)
            {
                return categories.AsPaged(options.CurrentPage, options.PageSize, totalCount, c => new CategoryDto
                {
                    Id = c.Id,
                    ArabicName = c.ArabicName,
                    EnglishName = c.EnglishName,
                    IsVisible = c.IsVisible
                });
            }

            var categoriesImagesByCategoryId = await _unitOfWork.CategoryImagesRepository.GetCategoriesImagesGroupedByIdAsync(categories.Select(c => c.Id), cancellationToken);

            return categories.AsPaged(options.CurrentPage, options.PageSize, totalCount, c =>
            {
                var categoryImage = categoriesImagesByCategoryId[c.Id];

                return new CategoryDto
                {
                    Id = c.Id,
                    IsVisible = c.IsVisible,
                    ArabicName = c.ArabicName,
                    EnglishName = c.EnglishName,
                    Image = new CategoryImageDto
                    {
                        Id = categoryImage.Id,
                        OriginalImage = _appImageHelper.GenerateImageDto(categoryImage.OriginalImage),
                        MediumImage = _appImageHelper.GenerateImageDto(categoryImage.MediumImage),
                        MobileImage = _appImageHelper.GenerateImageDto(categoryImage.MobileImage)
                    }
                };
            });
        }

        public async Task<string> GetLocalizedCategoryNameAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);

            return _loggedInUser.IsCultureArabic ? category.ArabicName : category.EnglishName;
        }
    }
}
