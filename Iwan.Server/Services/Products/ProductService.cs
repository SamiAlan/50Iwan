using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Media;
using Iwan.Server.Domain.Products;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Options;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Products
{
    [Injected(ServiceLifetime.Scoped, typeof(IProductService))]
    public class ProductService : IProductService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IFileProvider _fileProvider;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;
        protected readonly IQuerySettingService _querySettingService;
        protected readonly IImageService _imageService;
        protected readonly IImageManipulatorService _imageManipulatorService;

        public ProductService(IFileProvider fileProvider, IUnitOfWork unitOfWork, IImageService imageService,
            ILoggedInUserProvider loggedInUserProvider, IQuerySettingService querySettingService,
            IImageManipulatorService imageManipulatorService)
        {
            _fileProvider = fileProvider;
            _unitOfWork = unitOfWork;
            _loggedInUserProvider = loggedInUserProvider;
            _querySettingService = querySettingService;
            _imageService = imageService;
            _imageManipulatorService = imageManipulatorService;
        }

        public async Task<Product> AddProductAsync(AddProductDto productToAdd, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.ProductsRepository.ProductNumberExistsAsync(productToAdd.Number, cancellationToken))
                throw new AlreadyExistException(Responses.Products.NumberAlreadyExists,
                    productToAdd.GetPropertyPath(p => p.Number), productToAdd.Number);

            // Make sure all categories exist and they are attached sub categories
            if (productToAdd.CategoriesIds?.Any() ?? false)
            {
                var allCategoriesExist = await _unitOfWork.CategoriesRepository.ExistAndAttachedSubCategoriesAsync
                (productToAdd.CategoriesIds, cancellationToken);

                if (!allCategoriesExist)
                    throw new BadRequestException(Responses.Products.CategoriesNotExist,
                        productToAdd.GetPropertyPath(p => p.CategoriesIds));
            }

            TempImage tempMainImage = null;
            if (productToAdd.MainImage != null)
            {
                // Make sure the main image exists as temp image
                tempMainImage = await _unitOfWork.TempImagesRepository.FindAsync(productToAdd.MainImage.Image.Id, cancellationToken);

                if (tempMainImage == null)
                    throw new NotFoundException(Responses.Products.MainImageNotFound,
                        productToAdd.GetPropertyPath(p => p.MainImage));
            }

            // Make sure all images exist as temp images
            var isThereUploadedImages = productToAdd.Images?.Any() ?? false;

            if (isThereUploadedImages)
            {
                var imagesIds = productToAdd.Images.Select(i => i.Image.Id).ToList();

                var allImagesExist = await _unitOfWork.TempImagesRepository.ExistAsync(imagesIds, cancellationToken);

                if (!allImagesExist)
                    throw new NotFoundException(Responses.Images.NotAllImagesExist);
            }

            string vendorId = null;
            if (!productToAdd.VendorId.IsNullOrEmptyOrWhiteSpaceSafe())
            {
                var vendor = await _unitOfWork.VendorsRepository.FindAsync(productToAdd.VendorId, cancellationToken);

                if (vendor == null)
                    throw new NotFoundException(Responses.Vendors.VendorNotFound);

                vendorId = vendor.Id;
            }

            var addedProduct = new Product
            {
                EnglishName = productToAdd.EnglishName,
                EnglishDescription = productToAdd.EnglishDescription,
                ArabicName = productToAdd.ArabicName,
                ArabicDescription = productToAdd.ArabicDescription,
                Number = productToAdd.Number,
                Age = productToAdd.Age,
                Price = productToAdd.Price,
                StockQuantity = productToAdd.StockQuantity,
                IsVisible = productToAdd.IsVisible,
                DimensionsInEnglish = productToAdd.DimensionsInEnglish,
                DimensionsInArabic = productToAdd.DimensionsInArabic,
                MakerArabicName = productToAdd.MakerArabicName,
                MakerEnglishName = productToAdd.MakerEnglishName,
                VendorId = vendorId,
                HasMainImage = tempMainImage != null
            };

            if (productToAdd.ColorTypeId.EqualsEnum(ColorType.Custom))
            {
                addedProduct.ColorType = ColorType.Custom;
                addedProduct.ColorCode = productToAdd.ColorCode;
            }
            else if (addedProduct.ColorTypeId.EqualsEnum(ColorType.FromParent))
            {
                addedProduct.ColorType = ColorType.FromParent;
            }
            else
                addedProduct.ColorType = ColorType.NoChange;


            await _unitOfWork.ProductsRepository.AddAsync(addedProduct, cancellationToken);

            if (productToAdd.CategoriesIds?.Any() ?? false)
            {
                await _unitOfWork.ProductCategoriesRepository.AddRangeAsync(productToAdd.CategoriesIds.Select(cId => new ProductCategory
                {
                    ProductId = addedProduct.Id,
                    CategoryId = cId
                }), cancellationToken);

                var categories = await _unitOfWork.CategoriesRepository.GetAsync(productToAdd.CategoriesIds, cancellationToken);

                foreach (var category in categories)
                    category.HasRelatedProducts = true;
            }

            if (productToAdd.States?.Any() ?? false)
            {
                await _unitOfWork.ProductStatesRepository.AddRangeAsync(productToAdd.States.Select(s => new ProductState
                {
                    ArabicName = s.ArabicName,
                    EnglishName = s.EnglishName,
                    ProductId = addedProduct.Id
                }), cancellationToken);
            }

            // Add the main product image
            if (tempMainImage != null)
                await AddMainImageFromTempImageAsync(addedProduct.Id, tempMainImage, cancellationToken);

            if (productToAdd.Images?.Any() ?? false)
                // Add the product images (original images and images derived from the cropped version)
                await AddTempImagesToProductAsync(addedProduct.Id, productToAdd.Images, cancellationToken);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return addedProduct;
        }

        public async Task<ProductState> AddProductStateAsync(AddProductStateDto state, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(state.ProductId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var addedState = await _unitOfWork.ProductStatesRepository.AddAsync(new ProductState
            {
                ArabicName = state.ArabicName,
                EnglishName = state.EnglishName,
                ProductId = state.ProductId,
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            return addedState;
        }

        private async Task<ProductMainImage> AddMainImageFromTempImageAsync(string productId, TempImage croppedTempImage, CancellationToken cancellationToken = default)
        {
            var imagesSettings = await _querySettingService.GetProductsImagesSettingsAsync(cancellationToken);
            var watermarkSettings = await _querySettingService.GetWatermarkImageSettingsAsync(cancellationToken);
            var virtualPathToProductsImages = _fileProvider.Combine(AppDirectories.Images.SELF, AppDirectories.Images.Products);
            var tempCroppedImageFullPath = _fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.FileName);

            var croppedImageExtension = _fileProvider.GetFileExtension(croppedTempImage.FileName);
            (var productCroppedImageFullPath, var productCroppedImageFileName) = _fileProvider.FormNewFilePath(virtualPathToProductsImages, ImagesPostfixes.ProductCropped, croppedImageExtension);

            var addedCroppedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
            {
                FileName = productCroppedImageFileName,
                MimeType = croppedImageExtension.ToMimeType(),
                VirtualPath = virtualPathToProductsImages,
            }, cancellationToken);

            Image addedMediumImage;
            Image addedSmallImage;
            Image addedMobileImage;

            if (watermarkSettings.ShouldAddWatermark && !watermarkSettings.WatermarkImageId.IsNullOrEmptyOrWhiteSpaceSafe())
            {
                var watermarkImageBytes = await _imageManipulatorService.GetBytesAsync(_fileProvider.CombineWithRoot(watermarkSettings.WatermarkImage.VirtualPath, watermarkSettings.WatermarkImage.FileName), cancellationToken);

                addedMediumImage = await _imageService.ResizeAndAddVersionOfImageWithWatermarkAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, watermarkImageBytes, watermarkSettings.Opacity, true, cancellationToken);
                addedSmallImage = await _imageService.ResizeAndAddVersionOfImageWithWatermarkAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductSmall, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, watermarkImageBytes, watermarkSettings.Opacity, true, cancellationToken);
                addedMobileImage = await _imageService.ResizeAndAddVersionOfImageWithWatermarkAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, watermarkImageBytes, watermarkSettings.Opacity, true, cancellationToken);
            }
            else
            {
                addedMediumImage = await _imageService.ResizeAndAddVersionOfImageAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, true, cancellationToken);
                addedSmallImage = await _imageService.ResizeAndAddVersionOfImageAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductSmall, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, true, cancellationToken);
                addedMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, true, cancellationToken);
            }


            var addedMainImage = await _unitOfWork.ProductMainImagesRepository.AddAsync(new ProductMainImage
            {
                OriginalImageId = addedCroppedImage.Id,
                MediumImageId = addedMediumImage.Id,
                MobileImageId = addedMobileImage.Id,
                SmallImageId = addedSmallImage.Id,
                ProductId = productId
            }, cancellationToken);

            _fileProvider.MoveFile(tempCroppedImageFullPath, productCroppedImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.SmallVersionFileName));

            return addedMainImage;
        }

        public async Task<ProductImage> AddProductImageAsync(string productId, AddProductImageDto imageToAdd, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var imageExist = await _unitOfWork.TempImagesRepository.ExistAsync(imageToAdd.Image.Id, cancellationToken);

            if (!imageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var productImage = await AddTempImageToProductAsync(product.Id, imageToAdd, cancellationToken);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            return productImage;
        }

        public async Task<ProductCategory> AddProductToCategoryAsync(string productId, string categoryId, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var category = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);

            if (category == null)
                throw new NotFoundException(Responses.Categories.CategoryNotExist);

            var categoryName = _loggedInUserProvider.IsCultureArabic ? category.ArabicName : category.EnglishName;

            var alreadyBelongToCategory = await _unitOfWork.ProductCategoriesRepository.ProductBelongToCategoryAsync(product.Id, category.Id, cancellationToken);

            if (alreadyBelongToCategory)
                throw new ConflictException(Responses.Products.AlreadyBelongToCategory,
                    _loggedInUserProvider.IsCultureArabic ? product.ArabicName : product.EnglishName,
                    _loggedInUserProvider.IsCultureArabic ? category.ArabicName : category.EnglishName);

            var categoryHasProducts = await _unitOfWork.ProductCategoriesRepository.AnyAsync(p => p.CategoryId == category.Id, cancellationToken);

            if (!categoryHasProducts)
            {
                category.HasRelatedProducts = true;
            }

            var productCategory = new ProductCategory
            {
                CategoryId = category.Id,
                ProductId = product.Id
            };

            await _unitOfWork.ProductCategoriesRepository.AddAsync(productCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return productCategory;
        }

        public async Task<Product> EditProductAsync(EditProductDto editedProduct, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(editedProduct.Id, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            if (product.Number != editedProduct.Number && await _unitOfWork.ProductsRepository.ProductNumberExistsAsync(editedProduct.Number, cancellationToken))
                throw new AlreadyExistException(Responses.Products.NumberAlreadyExists,
                    editedProduct.GetPropertyPath(p => p.Number), editedProduct.Number);

            if (product.VendorId != editedProduct.VendorId)
            {
                if (editedProduct.VendorId.IsNullOrEmptyOrWhiteSpaceSafe())
                    product.VendorId = null;
                else
                {
                    if (!await _unitOfWork.VendorsRepository.ExistAsync(editedProduct.VendorId, cancellationToken))
                        throw new NotFoundException(Responses.Vendors.VendorNotFound,
                            editedProduct.GetPropertyPath(p => p.VendorId));

                    product.VendorId = editedProduct.VendorId;
                }
            }

            if (editedProduct.IsVisible && !await _unitOfWork.ProductsRepository.HasMainImageAsync(product.Id, cancellationToken))
                throw new BadRequestException(Responses.Products.ProductHasNotMainImage,
                    editedProduct.GetPropertyPath(p => p.IsVisible));

            product.Number = editedProduct.Number;
            product.ArabicName = editedProduct.ArabicName;
            product.EnglishName = editedProduct.EnglishName;
            product.ArabicDescription = editedProduct.ArabicDescription;
            product.EnglishDescription = editedProduct.EnglishDescription;
            product.MakerEnglishName = editedProduct.MakerEnglishName;
            product.MakerArabicName = editedProduct.MakerArabicName;
            product.Age = editedProduct.Age;
            product.Price = editedProduct.Price;
            product.StockQuantity = editedProduct.StockQuantity;
            product.IsVisible = editedProduct.IsVisible;
            product.DimensionsInArabic = editedProduct.DimensionsInArabic;
            product.DimensionsInEnglish = editedProduct.DimensionsInEnglish;

            if (editedProduct.ColorTypeId.EqualsEnum(ColorType.Custom))
            {
                product.ColorType = ColorType.Custom;
                product.ColorCode = editedProduct.ColorCode;
            }
            else if (editedProduct.ColorTypeId.EqualsEnum(ColorType.FromParent))
            {
                product.ColorType = ColorType.FromParent;
            }
            else
                product.ColorType = ColorType.NoChange;

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return product;
        }

        public async Task<ProductMainImage> ChangeProductMainImageAsync(ChangeProductMainImageDto editedImage, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(editedImage.ProductId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var productMainImage = await ChangeProductMainImageAsync(product.Id, editedImage.Image.Id, cancellationToken);

            product.HasMainImage = true;
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return productMainImage;
        }

        public async Task<ProductMainImage> ChangeProductMainImageAsync(string productId, string tempImageId, CancellationToken cancellationToken = default)
        {
            var croppedTempImage = await _unitOfWork.TempImagesRepository.FindAsync(tempImageId, cancellationToken);

            if (croppedTempImage == null)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var currentMainImage = await _unitOfWork.ProductMainImagesRepository
                .GetProductMainImageAsync(productId, cancellationToken);

            var addedMainImage = await AddMainImageFromTempImageAsync(productId, croppedTempImage, cancellationToken);

            if (currentMainImage != null)
            {
                var images = new Image[] { currentMainImage.OriginalImage, currentMainImage.MediumImage,
                    currentMainImage.SmallImage, currentMainImage.MobileImage};

                _imageService.DeleteImagesFiles(images);
                _unitOfWork.ImagesRepository.DeleteRange(images);

                _unitOfWork.ProductMainImagesRepository.Delete(currentMainImage);
            }

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return addedMainImage;
        }

        public async Task DeleteProductAsync(string productId, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var productImages = await _unitOfWork.ProductImagesRepository.GetImagesWithImageModelsAsync(product.Id, cancellationToken);
            var mainProductImage = await _unitOfWork.ProductMainImagesRepository.GetProductMainImageAsync(product.Id, cancellationToken);

            var imagesToDelete = new List<Image>();

            if (mainProductImage != null)
            {
                imagesToDelete.AddRange(new List<Image>
                {
                    mainProductImage.OriginalImage, mainProductImage.MediumImage,
                    mainProductImage.SmallImage, mainProductImage.MobileImage
                });
            }

            foreach (var productImage in productImages)
            {
                imagesToDelete.AddRange(new List<Image>
                {
                    productImage.OriginalImage, productImage.MediumImage,
                    productImage.SmallImage, productImage.MobileImage,
                });
            }

            // Get related categories having only the product under them and mark them
            // as not having any related products
            var relatedCategories = await _unitOfWork.ProductCategoriesRepository.GetCategoriesOfProductAsync(product.Id, true, cancellationToken);

            if (relatedCategories.Any())
            {
                foreach (var category in relatedCategories)
                {
                    category.HasRelatedProducts = false;
                }
            }

            _imageService.DeleteImagesFiles(imagesToDelete);
            _unitOfWork.ImagesRepository.DeleteRange(imagesToDelete);
            _unitOfWork.ProductImagesRepository.DeleteRange(productImages);

            if (mainProductImage != null)
                _unitOfWork.ProductMainImagesRepository.Delete(mainProductImage);

            _unitOfWork.ProductsRepository.Delete(product);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task DeleteProductImageAsync(string productImageId, CancellationToken cancellationToken)
        {
            var productImage = await _unitOfWork.ProductImagesRepository.GetImageWithImageModelsAsync(productImageId, cancellationToken);

            if (productImage == null)
                throw new NotFoundException(Responses.Products.ProductImageNotFound);

            var images = new List<Image>()
                { productImage.OriginalImage, productImage.MediumImage,
                  productImage.SmallImage, productImage.MobileImage, };

            _imageService.DeleteImagesFiles(images);
            _unitOfWork.ImagesRepository.DeleteRange(images);

            _unitOfWork.ProductImagesRepository.Delete(productImage);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task DeleteProductStateAsync(string stateId, CancellationToken cancellationToken = default)
        {
            var state = await _unitOfWork.ProductStatesRepository.FindAsync(stateId, cancellationToken);

            if (state == null)
                throw new NotFoundException(Responses.Products.StateNotFound);

            _unitOfWork.ProductStatesRepository.Delete(state);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task<(string, string)> RemoveProductCategoryAsync(string productCategoryId, CancellationToken cancellationToken = default)
        {
            var productCategory = await _unitOfWork.ProductCategoriesRepository.FindAsync(productCategoryId, cancellationToken);

            if (productCategory == null)
                throw new NotFoundException(Responses.Products.ProductCategoryNotFound);

            var category = productCategory.Category;

            var relatedProductsToCategoryCount = await _unitOfWork.ProductCategoriesRepository.CountAsync(p => p.CategoryId == category.Id,
                cancellationToken);

            if (relatedProductsToCategoryCount < 2)
            {
                category.HasRelatedProducts = false;
            }

            _unitOfWork.ProductCategoriesRepository.Delete(productCategory);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return (productCategory.ProductId, productCategory.CategoryId);
        }

        #region Helpers

        private async Task<ProductImage> AddTempImageToProductAsync(string productId, AddProductImageDto productImage, CancellationToken cancellationToken)
        {
            return (await AddTempImagesToProductAsync(productId, new List<AddProductImageDto> { productImage }, cancellationToken)).FirstOrDefault();
        }

        private async Task<IEnumerable<ProductImage>> AddTempImagesToProductAsync(string productId, IList<AddProductImageDto> productImages, CancellationToken cancellationToken)
        {
            var croppedTempImages = (await _unitOfWork.TempImagesRepository.GetAsync(productImages.Select(i => i.Image.Id), cancellationToken)).ToArray();

            var imagesSettings = await _querySettingService.GetProductsImagesSettingsAsync(cancellationToken);
            var watermarkSettings = await _querySettingService.GetWatermarkImageSettingsAsync(cancellationToken);

            var fileMovements = new List<(string, string)>();
            var croppedImageIds = new string[productImages.Count];
            var mediumImageIds = new string[productImages.Count];
            var thumbnailImageIds = new string[productImages.Count];
            var smallImageIds = new string[productImages.Count];
            var mobileImageIds = new string[productImages.Count];
            var smallVersionTempFilesToDelete = new List<string>();
            byte[] watermarkImageBytes = Array.Empty<byte>();
            var virtualPathToProductsImages = _fileProvider.Combine(AppDirectories.Images.SELF, AppDirectories.Images.Products);

            if (watermarkSettings.ShouldAddWatermark)
            {
                watermarkImageBytes = await _imageManipulatorService.GetBytesAsync(_fileProvider.CombineWithRoot(watermarkSettings.WatermarkImage.VirtualPath, watermarkSettings.WatermarkImage.FileName), cancellationToken);
            }

            for (int i = 0; i < productImages.Count; i++)
            {
                var croppedTempImage = croppedTempImages[i];

                // Form cropped temp image full path
                var tempCroppedImageFullPath = _fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.FileName);

                var croppedImageExtension = _fileProvider.GetFileExtension(croppedTempImage.FileName);

                (var productCroppedImageFullPath, var productCroppedImageFileName) = _fileProvider.FormNewFilePath(virtualPathToProductsImages, ImagesPostfixes.ProductCropped, croppedImageExtension);

                var addedCroppedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
                {
                    FileName = productCroppedImageFileName,
                    MimeType = croppedImageExtension.ToMimeType(),
                    VirtualPath = virtualPathToProductsImages,
                }, cancellationToken);

                Image addedMediumImage;
                Image addedSmallImage;
                Image addedMobileImage;

                if (watermarkSettings.ShouldAddWatermark)
                {
                    addedMediumImage = await _imageService.ResizeAndAddVersionOfImageWithWatermarkAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, watermarkImageBytes, watermarkSettings.Opacity, true, cancellationToken);
                    addedSmallImage = await _imageService.ResizeAndAddVersionOfImageWithWatermarkAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductSmall, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, watermarkImageBytes, watermarkSettings.Opacity, true, cancellationToken);
                    addedMobileImage = await _imageService.ResizeAndAddVersionOfImageWithWatermarkAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, watermarkImageBytes, watermarkSettings.Opacity, true, cancellationToken);
                }
                else
                {
                    addedMediumImage = await _imageService.ResizeAndAddVersionOfImageAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, true, cancellationToken);
                    addedSmallImage = await _imageService.ResizeAndAddVersionOfImageAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductSmall, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, true, cancellationToken);
                    addedMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(tempCroppedImageFullPath, virtualPathToProductsImages, croppedImageExtension, ImagesPostfixes.ProductMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, true, cancellationToken);
                }

                fileMovements.Add(ValueTuple.Create(tempCroppedImageFullPath, productCroppedImageFullPath));

                croppedImageIds[i] = addedCroppedImage.Id;
                mediumImageIds[i] = addedMediumImage.Id;
                smallImageIds[i] = addedSmallImage.Id;
                mobileImageIds[i] = addedMobileImage.Id;

                smallVersionTempFilesToDelete.Add(_fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.SmallVersionFileName));
            }

            var addedImages = await AddImagesToProductAsync(productId, croppedImageIds, mediumImageIds,
                thumbnailImageIds, smallImageIds, mobileImageIds, cancellationToken);

            _unitOfWork.TempImagesRepository.DeleteRange(croppedTempImages);

            _fileProvider.MoveFiles(fileMovements, true);
            _fileProvider.DeleteFiles(smallVersionTempFilesToDelete);

            return addedImages;
        }

        private async Task<IEnumerable<ProductImage>> AddImagesToProductAsync(string productId, string[] croppedImagesIds,
            string[] mediumImagesIds, string[] thumbnailImagesIds, string[] smallImagesIds,
            string[] mobileImagesIds, CancellationToken cancellationToken)
        {
            var productImages = new List<ProductImage>();

            for (int i = 0; i < croppedImagesIds.Length; i++)
            {
                productImages.Add(new ProductImage
                {
                    OriginalImageId = croppedImagesIds[i],
                    MediumImageId = mediumImagesIds[i],
                    SmallImageId = smallImagesIds[i],
                    MobileImageId = mobileImagesIds[i],
                    ProductId = productId
                });
            }

            await _unitOfWork.ProductImagesRepository.AddRangeAsync(productImages, cancellationToken);

            return productImages;
        }

        #endregion
    }
}
