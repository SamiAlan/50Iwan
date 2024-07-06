using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Catelog;
using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Media;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Catalog
{
    [Injected(ServiceLifetime.Scoped, typeof(ICategoryService))]
    public class CategoryService : ICategoryService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IFileProvider _fileProvider;
        protected readonly IImageService _imageService;
        protected readonly IQuerySettingService _querySettingService;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public CategoryService(IUnitOfWork unitOfWork, IFileProvider fileProvider,
            IImageService imageService, IQuerySettingService querySettingService,
            ILoggedInUserProvider loggedInUserProvider)
        {
            _unitOfWork = unitOfWork;
            _fileProvider = fileProvider;
            _imageService = imageService;
            _querySettingService = querySettingService;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<Category> AddCategoryAsync(AddCategoryDto categoryToAdd, CancellationToken cancellationToken = default)
        {
            var categoryWithSameName = await _unitOfWork.CategoriesRepository
                    .FirstOrDefaultAsync(c => c.ArabicName.ToLower() == categoryToAdd.ArabicName.ToLower() || c.EnglishName.ToLower() == categoryToAdd.EnglishName.ToLower(), cancellationToken);

            if (categoryWithSameName != null)
                throw new AlreadyExistException(Responses.Categories.CategorySameNameAlreadyExist,
                    categoryToAdd.ArabicName.EqualsIgnoreCase(categoryWithSameName.ArabicName) ?
                        categoryToAdd.GetPropertyPath(c => c.ArabicName) : categoryToAdd.GetPropertyPath(c => c.EnglishName));

            // Check if temp images already exist
            var tempImageExist = await _unitOfWork.TempImagesRepository.ExistAsync(categoryToAdd.Image.Image.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var newCategory = new Category
            {
                ArabicName = categoryToAdd.ArabicName,
                EnglishName = categoryToAdd.EnglishName,
                IsVisible = categoryToAdd.IsVisible
            };

            if (categoryToAdd.ColorTypeId.EqualsEnum(ColorType.Custom))
            {
                newCategory.ColorType = ColorType.Custom;
                newCategory.ColorCode = categoryToAdd.ColorCode;
            }
            else if (categoryToAdd.ColorTypeId.EqualsEnum(ColorType.FromParent))
            {
                newCategory.ColorType = ColorType.FromParent;
            }
            else
                newCategory.ColorType = ColorType.NoChange;

            if (!categoryToAdd.ParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
            {
                var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(categoryToAdd.ParentCategoryId, cancellationToken: cancellationToken);

                if (parentCategory is null)
                    throw new BadRequestException(Responses.Categories.ParentCategoryNotExist,
                        categoryToAdd.GetPropertyPath(c => c.ParentCategoryId));

                if (parentCategory.IsSubCategory)
                    throw new BadRequestException(Responses.Categories.CantExtendSubCategory,
                        categoryToAdd.GetPropertyPath(c => c.ParentCategoryId));

                if (!parentCategory.HasSubCategories)
                {
                    parentCategory.HasSubCategories = true;
                }

                newCategory.ParentCategoryId = parentCategory.Id;
                newCategory.IsSubCategory = true;
            }

            await _unitOfWork.CategoriesRepository.AddAsync(newCategory, cancellationToken);

            await GenerateAndAddCategoryImageAsync
                (newCategory.Id, categoryToAdd.Image.Image.Id, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

            return newCategory;
        }

        public async Task DeleteCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);

            if (category is null)
                throw new NotFoundException(Responses.Categories.CategoryNotExist);

            if (category.IsSubCategory)
            {
                var thereAreOtherSubCategories = await _unitOfWork.CategoriesRepository.AnyAsync(c => c.ParentCategoryId == category.ParentCategoryId && c.Id != category.Id, cancellationToken);

                if (!thereAreOtherSubCategories)
                {
                    var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(category.ParentCategoryId, cancellationToken);

                    parentCategory.HasSubCategories = false;
                }
            }

            // Delete category image
            var categoryImage = await _unitOfWork.CategoryImagesRepository.GetByCategoryIncludingImagesAsync(category.Id, cancellationToken);

            if (categoryImage != null)
            {
                var images = new Image[] { categoryImage.OriginalImage, categoryImage.MediumImage,
                    categoryImage.MobileImage };

                _imageService.DeleteImagesFiles(images);
                _unitOfWork.ImagesRepository.DeleteRange(images);

                _unitOfWork.CategoryImagesRepository.Delete(categoryImage);
            }

            // If there are subcategories then make their parent id property set to null
            if (!category.IsSubCategory)
            {
                var subCategories = await _unitOfWork.CategoriesRepository.GetSubCategoriesAsync(category.Id, cancellationToken);

                if (subCategories.Any())
                {
                    foreach (var subCategory in subCategories)
                    {
                        subCategory.ParentCategoryId = null;
                        subCategory.IsSubCategory = false;
                        subCategory.IsVisible = false;
                    }
                }
            }

            _unitOfWork.CategoriesRepository.Delete(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<Category> EditCategoryAsync(EditCategoryDto editedCategory, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.CategoriesRepository.FindAsync(editedCategory.Id, cancellationToken);

            if (category is null)
                throw new NotFoundException(Responses.Categories.CategoryNotExist);

            if (category.ArabicName != editedCategory.ArabicName || category.EnglishName != editedCategory.EnglishName)
            {
                var categoryWithSameName = await _unitOfWork.CategoriesRepository.FirstOrDefaultAsync
                (c => c.ArabicName.ToLower() == editedCategory.ArabicName.ToLower() || c.EnglishName.ToLower() == editedCategory.EnglishName.ToLower() && c.Id != category.Id, cancellationToken);

                if (categoryWithSameName != null)
                    throw new AlreadyExistException(Responses.Categories.CategorySameNameAlreadyExist,
                        editedCategory.ArabicName.EqualsIgnoreCase(categoryWithSameName.ArabicName) ?
                            editedCategory.GetPropertyPath(c => editedCategory.ArabicName) :
                            editedCategory.GetPropertyPath(c => editedCategory.EnglishName));
            }

            category.SetIfNotEqual(c => c.ArabicName, editedCategory.ArabicName);
            category.SetIfNotEqual(c => c.EnglishName, editedCategory.EnglishName);
            category.SetIfNotEqual(c => c.IsVisible, editedCategory.IsVisible);

            if (editedCategory.ColorTypeId.EqualsEnum(ColorType.Custom))
            {
                category.ColorType = ColorType.Custom;
                category.ColorCode = editedCategory.ColorCode;
            }
            else if (editedCategory.ColorTypeId.EqualsEnum(ColorType.FromParent))
            {
                category.ColorType = ColorType.FromParent;
            }
            else
                category.ColorType = ColorType.NoChange;

            // If parent category ids are different then there is a change that should be dealt with
            if (category.ParentCategoryId != editedCategory.ParentCategoryId)
            {
                // Either the category was attached and got detachted
                if (!category.ParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe() && editedCategory.ParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    // if there aren't any attached products to the sub-category then mark it as parent
                    //if (!await _unitOfWork.ProductCategoriesRepository.AnyAsync(c => c.CategoryId == category.Id, cancellationToken))
                    category.IsSubCategory = false;

                    var thereAreOtherSubCategories = await _unitOfWork.CategoriesRepository.AnyAsync(c => c.ParentCategoryId == category.ParentCategoryId && c.Id != category.Id, cancellationToken);

                    if (!thereAreOtherSubCategories)
                    {
                        var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(category.ParentCategoryId, cancellationToken);

                        parentCategory.HasSubCategories = false;
                    }

                    category.ParentCategoryId = null;
                }
                // or it was detached and got attached
                else if (category.ParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe() && !editedCategory.ParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    // if it was a parent category that has other categories as children
                    if (await _unitOfWork.CategoriesRepository.AnyAsync(c => c.ParentCategoryId == category.Id, cancellationToken))
                        throw new BadRequestException(Responses.Categories.CategoryHasSubCategories);

                    var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(editedCategory.ParentCategoryId, cancellationToken);

                    if (parentCategory is null)
                        throw new NotFoundException(Responses.Categories.CategoryNotExist);

                    if (parentCategory.Id == category.Id)
                        throw new ConflictException(Responses.Categories.CantExtendSelf,
                            editedCategory.GetPropertyPath(c => c.ParentCategoryId));

                    if (parentCategory.IsSubCategory)
                        throw new BadRequestException(Responses.Categories.CantExtendSubCategory,
                            editedCategory.GetPropertyPath(c => c.ParentCategoryId));

                    if (!parentCategory.HasSubCategories)
                    {
                        parentCategory.HasSubCategories = true;
                    }

                    category.ParentCategoryId = parentCategory.Id;
                    category.IsSubCategory = true;
                }
                // Was attached and got attached to another category
                else
                {
                    var newParentCategory = await _unitOfWork.CategoriesRepository.FindAsync(editedCategory.ParentCategoryId, cancellationToken);

                    if (newParentCategory is null)
                        throw new NotFoundException(Responses.Categories.CategoryNotExist);

                    if (newParentCategory.Id == category.Id)
                        throw new ConflictException(Responses.Categories.CantExtendSelf,
                            editedCategory.GetPropertyPath(c => c.ParentCategoryId));

                    if (newParentCategory.IsSubCategory)
                        throw new BadRequestException(Responses.Categories.CantExtendSubCategory,
                           editedCategory.GetPropertyPath(c => c.ParentCategoryId));

                    var oldParentCategory = await _unitOfWork.CategoriesRepository.FindAsync(category.ParentCategoryId, cancellationToken);

                    if (oldParentCategory is null)
                        throw new NotFoundException(Responses.Categories.CategoryNotExist);

                    var oldParentHasOtherSubCategories = await _unitOfWork.CategoriesRepository.AnyAsync(c => c.Id != category.Id && c.ParentCategoryId == oldParentCategory.Id, cancellationToken);

                    if (!newParentCategory.HasSubCategories)
                    {
                        newParentCategory.HasSubCategories = true;
                    }

                    if (!oldParentHasOtherSubCategories)
                    {
                        oldParentCategory.HasSubCategories = false;
                    }

                    category.ParentCategoryId = newParentCategory.Id;
                }
            }

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return category;
        }

        public async Task<CategoryImage> ChangeCategoryImageAsync(string categoryId, string bacgroundlessCroppedTempImageId, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);

            if (category == null)
                throw new NotFoundException(Responses.Categories.CategoryNotExist);

            // Check if temp images already exist
            var tempImageExist = await _unitOfWork.TempImagesRepository.ExistAsync(bacgroundlessCroppedTempImageId, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var currentCategoryImage = await _unitOfWork.CategoryImagesRepository
                .GetByCategoryIncludingImagesAsync(category.Id, cancellationToken);

            var newCategoryImage = await GenerateAndAddCategoryImageAsync(categoryId, bacgroundlessCroppedTempImageId, cancellationToken);

            if (currentCategoryImage != null)
            {
                var images = new Image[] { currentCategoryImage.OriginalImage, currentCategoryImage.MediumImage,
                    currentCategoryImage.MobileImage};

                _imageService.DeleteImagesFiles(images);
                _unitOfWork.ImagesRepository.DeleteRange(images);

                _unitOfWork.CategoryImagesRepository.Delete(currentCategoryImage);
            }

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return newCategoryImage;
        }

        public async Task<CategoryImage> EditCategoryImageAsync(EditCategoryImageDto editedImage, CancellationToken cancellationToken = default)
        {
            var categoryImage = await _unitOfWork.CategoryImagesRepository.FindAsync(editedImage.Id, cancellationToken);

            if (categoryImage == null)
                throw new NotFoundException(Responses.Categories.CategoryImageNotFound);

            // Do some editions and save them
            // Has to be reconsidered whether to keep such functionality or not (which will be decided if
            // we store more category-image-related stuff or not

            return categoryImage;
        }

        #region Helpers

        private async Task<CategoryImage> GenerateAndAddCategoryImageAsync(string categoryId, string backgroundlessCroppedTempImageId, CancellationToken cancellationToken = default)
        {
            var backgroundlessCroppedTempImage = await _unitOfWork.TempImagesRepository.FindAsync(backgroundlessCroppedTempImageId, cancellationToken);

            var backgroundlessCroppedImageExtension = _fileProvider.GetFileExtension(backgroundlessCroppedTempImage.FileName);

            // Get images settings
            var imagesSettings = await _querySettingService.GetCategoriesImagesSettingsAsync(cancellationToken);

            // Virtual path to categories image directory
            var virtualPathToCategoriesImages = _fileProvider.Combine(AppDirectories.Images.SELF,
                AppDirectories.Images.Categories);

            // Form backgroundless cropped image full path and filename
            (var croppedBackgroundlessImageFullPath, var croppedBackgroundlessImageFileName) = _fileProvider.FormNewFilePath(virtualPathToCategoriesImages, ImagesPostfixes.CategoryBackgroundlessCropped, backgroundlessCroppedImageExtension);

            var addedBackgroundlessCroppedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
            {
                FileName = croppedBackgroundlessImageFileName,
                VirtualPath = virtualPathToCategoriesImages,
                MimeType = backgroundlessCroppedImageExtension.ToMimeType()
            }, cancellationToken);

            // Form backgroundless cropped temp image full path
            var backgroundlessCroppedTempImageFullPath = _fileProvider.CombineWithRoot(backgroundlessCroppedTempImage.VirtualPath, backgroundlessCroppedTempImage.FileName);

            var addedBackgroundlessMediumImage = await _imageService.ResizeAndAddVersionOfImageAsync(backgroundlessCroppedTempImageFullPath, virtualPathToCategoriesImages, backgroundlessCroppedImageExtension, ImagesPostfixes.CategoryBackgroundlessMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, true, cancellationToken);

            var addedBackgroundlessMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(backgroundlessCroppedTempImageFullPath, virtualPathToCategoriesImages, backgroundlessCroppedImageExtension, ImagesPostfixes.CategoryBackgroundlessMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, true, cancellationToken);

            // Add the category image as a model
            var categoryImage = await AddCategoryImageAsync(categoryId,
                addedBackgroundlessCroppedImage.Id, addedBackgroundlessMediumImage.Id,
                addedBackgroundlessMobileImage.Id, cancellationToken);

            _fileProvider.MoveFile(backgroundlessCroppedTempImageFullPath, croppedBackgroundlessImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(backgroundlessCroppedTempImage.VirtualPath, backgroundlessCroppedTempImage.SmallVersionFileName));
            _unitOfWork.TempImagesRepository.Delete(backgroundlessCroppedTempImage);

            return categoryImage;
        }

        private async Task<CategoryImage> AddCategoryImageAsync(string categoryId, string originalImageId, string mediumImageId,
            string backgroundlessMobileImageId, CancellationToken cancellationToken)
        {
            var categoryImage = new CategoryImage
            {
                CategoryId = categoryId,
                OriginalImageId = originalImageId,
                MediumImageId = mediumImageId,
                MobileImageId = backgroundlessMobileImageId
            };

            await _unitOfWork.CategoryImagesRepository.AddAsync(categoryImage, cancellationToken);

            return categoryImage;
        }

        #endregion
    }
}
