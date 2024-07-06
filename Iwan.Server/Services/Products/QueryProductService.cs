using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Server.Extensions;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Models;
using Iwan.Server.Models.Products;
using Iwan.Server.Options;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Options.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Products
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryProductService))]
    public class QueryProductService : IQueryProductService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAppImageHelper _appImageHelper;
        protected readonly ILoggedInUserProvider _loggedInUser;
        protected readonly IQuerySettingService _querySettingService;

        public QueryProductService(IUnitOfWork unitOfWork, IAppImageHelper appImageHelper,
            ILoggedInUserProvider loggedInUser, IQuerySettingService querySettingService)
        {
            _unitOfWork = unitOfWork;
            _appImageHelper = appImageHelper;
            _loggedInUser = loggedInUser;
            _querySettingService = querySettingService;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategoriesAsync(string productId, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var categories = (await _unitOfWork.ProductCategoriesRepository.GetByProductIdAsync(product.Id, true, cancellationToken));

            return categories.Select(c => new ProductCategoryDto
            {
                Id = c.Id,
                ArabicName = c.Category.ArabicName,
                EnglishName = c.Category.EnglishName,
                CategoryId = c.CategoryId
            });
        }

        public async Task<ProductImageDto> GetProductImageDetailsAsync(string productImageId, bool includeImages = true, CancellationToken cancellationToken = default)
        {
            var productImage = includeImages ? await _unitOfWork.ProductImagesRepository.GetImageWithImageModelsAsync(productImageId, cancellationToken)
                : await _unitOfWork.ProductImagesRepository.FindAsync(productImageId, cancellationToken);

            if (productImage == null)
                throw new NotFoundException(Responses.Products.ProductImageNotFound);

            if (!includeImages)
                return new ProductImageDto
                {
                    Id = productImage.Id,
                };

            return new ProductImageDto
            {
                Id = productImage.Id,
                OriginalImage = _appImageHelper.GenerateImageDto(productImage.OriginalImage),
                MediumImage = _appImageHelper.GenerateImageDto(productImage.MediumImage),
                SmallImage = _appImageHelper.GenerateImageDto(productImage.SmallImage),
                MobileImage = _appImageHelper.GenerateImageDto(productImage.MobileImage)
            };
        }

        public async Task<ProductCategoryDto> GetProductCategoryDetailsAsync(string productCategoryId, CancellationToken cancellationToken)
        {
            var productCategory = await _unitOfWork.ProductCategoriesRepository.FindAsync(productCategoryId, cancellationToken);

            if (productCategory == null)
                throw new NotFoundException(Responses.Products.ProductCategoryNotFound);

            var category = productCategory.Category;

            return new ProductCategoryDto
            {
                Id = productCategory.Id,
                ArabicName = category.ArabicName,
                EnglishName = category.EnglishName,
                CategoryId = category.Id
            };
        }

        public async Task<ProductDto> GetProductDetailsAsync(string productId, bool includeImages = true, bool includeCategories = true, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var states = (await _unitOfWork.ProductStatesRepository.GetStatesForProductAsync(product.Id, cancellationToken)).Select(s => new ProductStateDto
            {
                Id = s.Id,
                ArabicName = s.ArabicName,
                EnglishName = s.EnglishName
            });

            var detailsDto = new ProductDto
            {
                Id = product.Id,
                Number = product.Number,
                ArabicDescription = product.ArabicDescription,
                EnglishDescription = product.EnglishDescription,
                ArabicName = product.ArabicName,
                EnglishName = product.EnglishName,
                ColorCode = product.ColorCode,
                ColorTypeId = product.ColorTypeId,
                Age = product.Age,
                DimensionsInArabic = product.DimensionsInArabic,
                DimensionsInEnglish = product.DimensionsInEnglish,
                IsVisible = product.IsVisible,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                MakerArabicName = product.MakerArabicName,
                MakerEnglishName = product.MakerEnglishName,
                States = states.ToList(),
                HasMainImage = product.HasMainImage,
            };

            if (includeImages)
            {
                var productImages = await _unitOfWork.ProductImagesRepository.GetImagesWithImageModelsAsync(productId, cancellationToken);

                detailsDto.Images = productImages.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    OriginalImage = _appImageHelper.GenerateImageDto(i.OriginalImage),
                    MediumImage = _appImageHelper.GenerateImageDto(i.MediumImage),
                    SmallImage = _appImageHelper.GenerateImageDto(i.SmallImage),
                    MobileImage = _appImageHelper.GenerateImageDto(i.MobileImage)
                }).ToList();
            }

            if (includeCategories)
                detailsDto.ProductCategories = (await GetProductCategoriesAsync(productId, cancellationToken)).ToList();

            if (!product.VendorId.IsNullOrEmptyOrWhiteSpaceSafe())
            {
                var vendor = product.Vendor;

                detailsDto.Vendor = new VendorDto
                {
                    Id = vendor.Id,
                    BenefitPercent = vendor.BenefitPercent,
                    Name = vendor.Name,
                    ShowOwnProducts = vendor.ShowOwnProducts
                };
            }

            return detailsDto;
        }

        public async Task<PagedDto<ProductDto>> GetProductsAsync(AdminGetProductsOptions options, CancellationToken cancellationToken)
        {
            var imageSettings = await _querySettingService.GetProductsImagesSettingsAsync(cancellationToken);

            var query = _unitOfWork.ProductsRepository.Table
                .Include(p => p.Vendor).Include(p => p.States).OrderBy(p => p.Number).AsQueryable();

            if (options.Number.HasValue)
                query = query.Where(p => p.Number == options.Number.Value);

            else
            {
                if (options.OnlyVisible.HasValue)
                    query = query.Where(p => p.IsVisible == options.OnlyVisible.Value);

                if (options.HavingNoMainImage.HasValue)
                    query = query.Where(p => p.HasMainImage == !options.HavingNoMainImage.Value);

                if (options.OnlyNeedingResize.HasValue)
                {
                    if (options.OnlyNeedingResize.Value)
                        query = query.Where(p => p.LastResizeDate > imageSettings.UpdatedDateUtc);
                    else
                        query = query.Where(p => p.LastResizeDate < imageSettings.UpdatedDateUtc);
                }

                if (options.OnlyUnattached.HasValue)
                    query = query.Where(p => p.ProductCategories.Any() == options.OnlyUnattached.Value);

                if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    var loweredText = options.Text.ToLower();
                    query = query.Where(p => p.EnglishName.ToLower().Contains(loweredText) || p.ArabicName.ToLower().Contains(loweredText));
                }

                if (!options.UnderCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                    query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == options.UnderCategoryId));

                if (options.OnlyOwnedProducts.HasValue)
                    query = query.Where(p => options.OnlyOwnedProducts.Value ? p.VendorId != null : p.VendorId == null);

                if (!options.UnderVendorId.IsNullOrEmptyOrWhiteSpaceSafe())
                    query = query.Where(p => p.VendorId == options.UnderVendorId);
            }
            
            var totalCount = await query.CountAsync(cancellationToken);

            var products = await query.Skip(options.CurrentPage * options.PageSize).Take(options.PageSize).ToListAsync(cancellationToken);

            if (!options.IncludeMainImage)
                return products.AsPaged(options.CurrentPage, options.PageSize, totalCount, p =>
                {
                    var dto = new ProductDto
                    {
                        Id = p.Id,
                        ArabicName = p.ArabicName,
                        ArabicDescription = p.ArabicDescription,
                        EnglishName = p.EnglishName,
                        EnglishDescription = p.EnglishDescription,
                        Number = p.Number,
                        ColorCode = p.ColorCode,
                        ColorTypeId = p.ColorTypeId,
                        Price = p.Price,
                        IsVisible = p.IsVisible,
                        Age = p.Age,
                        DimensionsInArabic = p.DimensionsInArabic,
                        DimensionsInEnglish = p.DimensionsInEnglish,
                        StockQuantity = p.StockQuantity,
                        MakerArabicName = p.MakerArabicName,
                        MakerEnglishName = p.MakerEnglishName,
                        HasMainImage = p.HasMainImage,
                        RequiresResizing = p.LastResizeDate < imageSettings.UpdatedDateUtc,
                        States = p.States.Select(s => new ProductStateDto
                        {
                            Id = s.Id,
                            ArabicName = s.ArabicName,
                            EnglishName = s.EnglishName,
                            ProductId = s.ProductId
                        }).ToList()
                    };

                    if (!p.VendorId.IsNullOrEmptyOrWhiteSpaceSafe())
                        dto.Vendor = new VendorDto
                        {
                            Id = p.VendorId,
                            Name = p.Vendor.Name
                        };

                    return dto;
                });

            var productsMainImagesByProductId = await _unitOfWork.ProductMainImagesRepository.GetProductsImagesGroupedByIdAsync(products.Select(c => c.Id), cancellationToken);

            return products.AsPaged(options.CurrentPage, options.PageSize, totalCount, p =>
            {
                var dto = new ProductDto
                {
                    Id = p.Id,
                    ArabicName = p.ArabicName,
                    ArabicDescription = p.ArabicDescription,
                    EnglishName = p.EnglishName,
                    EnglishDescription = p.EnglishDescription,
                    Number = p.Number,
                    ColorCode = p.ColorCode,
                    ColorTypeId = p.ColorTypeId,
                    Price = p.Price,
                    IsVisible = p.IsVisible,
                    Age = p.Age,
                    DimensionsInArabic = p.DimensionsInArabic,
                    DimensionsInEnglish = p.DimensionsInEnglish,
                    StockQuantity = p.StockQuantity,
                    RequiresResizing = p.LastResizeDate < imageSettings.UpdatedDateUtc,
                    HasMainImage = p.HasMainImage,
                    MainImage = productsMainImagesByProductId.TryGetValue(p.Id, out var productMainImage) ? new ProductMainImageDto
                    {
                        Id = productMainImage.Id,
                        OriginalImage = _appImageHelper.GenerateImageDto(productMainImage.OriginalImage),
                        MediumImage = _appImageHelper.GenerateImageDto(productMainImage.MediumImage),
                        SmallImage = _appImageHelper.GenerateImageDto(productMainImage.SmallImage),
                        MobileImage = _appImageHelper.GenerateImageDto(productMainImage.MobileImage)
                    } : null
                };

                if (!p.VendorId.IsNullOrEmptyOrWhiteSpaceSafe())
                    dto.Vendor = new VendorDto
                    {
                        Id = p.VendorId,
                        Name = p.Vendor.Name
                    };

                return dto;
            });
        }

        public async Task<IEnumerable<ProductStateDto>> GetProductStatesAsync(string productId, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            return (await _unitOfWork.ProductStatesRepository.GetStatesForProductAsync(productId, cancellationToken))
                .Select(s => new ProductStateDto
                {
                    Id = s.Id,
                    ArabicName = s.ArabicName,
                    EnglishName = s.EnglishName
                });
        }

        public async Task<ProductStateDto> GetProductStateDetailsAsync(string stateId, CancellationToken cancellationToken = default)
        {
            var state = await _unitOfWork.ProductStatesRepository.FindAsync(stateId, cancellationToken);

            if (state == null)
                throw new NotFoundException(Responses.Products.StateNotFound);

            return new ProductStateDto
            {
                Id = state.Id,
                ArabicName = state.ArabicName,
                EnglishName = state.EnglishName,
                ProductId = state.ProductId
            };
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(GetAllProductsOptions options, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.ProductsRepository.Table;

            if (options.OnlyVisible.HasValue)
                query = query.Where(p => p.IsVisible == options.OnlyVisible.Value);

            if (options.OnlyUnattached.HasValue)
                query = query.Where(p => p.ProductCategories.Any() == options.OnlyUnattached.Value);

            if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
            {
                var loweredText = options.Text.ToLower();
                query = query.Where(p => p.EnglishName.ToLower().Contains(loweredText) || p.ArabicName.ToLower().Contains(loweredText));
            }

            if (!options.UnderSubCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == options.UnderSubCategoryId));

            if (!options.UnderParentCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(p => p.ProductCategories.Any(pc => pc.Category.ParentCategoryId == options.UnderParentCategoryId));

            var products = await query.ToListAsync(cancellationToken);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ArabicName = p.ArabicName,
                EnglishName = p.EnglishName,
                ColorCode = p.ColorCode,
                ColorTypeId = p.ColorTypeId,
                IsVisible = p.IsVisible
            }).ToList();
        }

        public async Task<PagedViewModel<ProductViewModel>> GetProductsAsync(GetProductsPageOptions options, CancellationToken cancellationToken = default)
        {
            var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(options.CategoryId, cancellationToken);

            var subCategory = await _unitOfWork.CategoriesRepository.FindAsync(options.SubCategoryId, cancellationToken);

            // Another way to retrieve products which could be examined later on
            var productsQuery = _unitOfWork.ProductsRepository.Where(p => p.IsVisible).OrderBy(p => p.Number).AsQueryable();

            if (subCategory != null)
            {
                productsQuery = productsQuery.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == subCategory.Id));
            }
            else
            {
                productsQuery = productsQuery.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == options.CategoryId || pc.Category.ParentCategoryId == options.CategoryId));
            }

            productsQuery = productsQuery.Distinct();

            var total = await productsQuery.CountAsync(cancellationToken);
            var products = await productsQuery.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize).ToListAsync(cancellationToken);
            var productsModels = new List<ProductViewModel>();

            var isCategoryColorSetAndValid = parentCategory.ColorType == ColorType.Custom && parentCategory.ColorCode.IsHtmlColor();
            var categoryColorCode = parentCategory.ColorCode;

            var isSubCategoryColorSetAndValid = subCategory != null && subCategory.ColorType == ColorType.Custom && subCategory.ColorCode.IsHtmlColor();
            var subCategoryColorCode = subCategory?.ColorCode;

            var productsIds = products.Select(p => p.Id);

            var productsImages = await _unitOfWork.ProductMainImagesRepository.GetProductsImagesGroupedByIdAsync(productsIds, cancellationToken);

            foreach (var product in products)
            {
                var productMainImage = productsImages[product.Id];

                var finalColor = string.Empty;
                var imageHasBackgroundColor = false;

                if (product.ColorType == ColorType.Custom)
                {
                    finalColor = product.ColorCode;
                    imageHasBackgroundColor = true;
                }
                else if (product.ColorType == ColorType.FromParent)
                {
                    if (isSubCategoryColorSetAndValid)
                    {
                        finalColor = subCategoryColorCode;
                        imageHasBackgroundColor = true;
                    }
                    else if (subCategory != null && subCategory.ColorType == ColorType.FromParent && isCategoryColorSetAndValid)
                    {
                        finalColor = categoryColorCode;
                        imageHasBackgroundColor = true;
                    }
                }

                var croppedImage = productMainImage.OriginalImage;
                var mediumImage = productMainImage.MediumImage;
                var smallImage = productMainImage.SmallImage;
                var mobileImage = productMainImage.MobileImage;

                productsModels.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Number = product.Number,
                    Age = product.Age,
                    Dimensions = _loggedInUser.IsCultureArabic ? product.DimensionsInArabic : product.DimensionsInEnglish,
                    Description = _loggedInUser.IsCultureArabic ? product.ArabicDescription : product.EnglishDescription,
                    Name = _loggedInUser.IsCultureArabic ? product.ArabicName : product.EnglishName,
                    Maker = _loggedInUser.IsCultureArabic ? product.MakerArabicName : product.MakerEnglishName,
                    IsAvailable = product.StockQuantity > 0,
                    Image = new ProductMainImageViewModel
                    {
                        Id = productMainImage.Id,
                        HasBackgroundColor = imageHasBackgroundColor,
                        ColorCode = finalColor,
                        OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                        MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                        SmallImage = _appImageHelper.GenerateImageModel(smallImage),
                        MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                    }
                });
            }

            return productsModels.AsPagedModel(options.CurrentPage, options.PageSize, total);
        }

        public async Task<ProductMainImageDto> GetProductMainImageDetailsAsync(string productMainImageId, CancellationToken cancellationToken = default)
        {
            var productMainImage = await _unitOfWork.ProductMainImagesRepository.GetAndIncludeImagesAsync(productMainImageId, cancellationToken);

            if (productMainImage is null)
                throw new NotFoundException(Responses.Products.MainImageNotFound);

            return new ProductMainImageDto
            {
                Id = productMainImage.Id,
                OriginalImage = _appImageHelper.GenerateImageDto(productMainImage.OriginalImage),
                MediumImage = _appImageHelper.GenerateImageDto(productMainImage.MediumImage),
                SmallImage = _appImageHelper.GenerateImageDto(productMainImage.SmallImage),
                MobileImage = _appImageHelper.GenerateImageDto(productMainImage.MobileImage)
            };
        }

        public async Task<IEnumerable<ProductImageDto>> GetProductImagesDetailsAsync(string id, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(id, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var productImages = await _unitOfWork.ProductImagesRepository.GetImagesWithImageModelsAsync(id, cancellationToken);

            return productImages.Select(i => new ProductImageDto
            {
                Id = i.Id,
                OriginalImage = _appImageHelper.GenerateImageDto(i.OriginalImage),
                MediumImage = _appImageHelper.GenerateImageDto(i.MediumImage),
                SmallImage = _appImageHelper.GenerateImageDto(i.SmallImage),
                MobileImage = _appImageHelper.GenerateImageDto(i.MobileImage)
            });
        }

        public async Task<ProductViewModel> GetProductDetailsViewModelAsync(string id, string categoryId, string subCategoryId, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(id, cancellationToken);

            if (!product.IsVisible) return null;

            var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);
            var subCategory = await _unitOfWork.CategoriesRepository.FindAsync(subCategoryId, cancellationToken);

            var isCategoryColorSetAndValid = parentCategory != null && parentCategory.ColorType == ColorType.Custom && parentCategory.ColorCode.IsHtmlColor();
            var categoryColorCode = parentCategory?.ColorCode;

            var isSubCategoryColorSetAndValid = subCategory != null && subCategory.ColorType == ColorType.Custom && subCategory.ColorCode.IsHtmlColor();
            var subCategoryColorCode = subCategory?.ColorCode;

            var productMainImage = await _unitOfWork.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(id, cancellationToken);

            var finalColor = string.Empty;
            var imageHasBackgroundColor = false;

            if (product.ColorType == ColorType.Custom)
            {
                finalColor = product.ColorCode;
                imageHasBackgroundColor = true;
            }
            else if (product.ColorType == ColorType.FromParent)
            {
                if (isSubCategoryColorSetAndValid)
                {
                    finalColor = subCategoryColorCode;
                    imageHasBackgroundColor = true;
                }
                else if (subCategory != null && subCategory.ColorType == ColorType.FromParent && isCategoryColorSetAndValid)
                {
                    finalColor = categoryColorCode;
                    imageHasBackgroundColor = true;
                }
            }

            var croppedImage = productMainImage.OriginalImage;
            var mediumImage = productMainImage.MediumImage;
            var smallImage = productMainImage.SmallImage;
            var mobileImage = productMainImage.MobileImage;

            return new ProductViewModel
            {
                Id = product.Id,
                Number = product.Number,
                Age = product.Age,
                Dimensions = _loggedInUser.IsCultureArabic ? product.DimensionsInArabic : product.DimensionsInEnglish,
                Description = _loggedInUser.IsCultureArabic ? product.ArabicDescription : product.EnglishDescription,
                Name = _loggedInUser.IsCultureArabic ? product.ArabicName : product.EnglishName,
                Maker = _loggedInUser.IsCultureArabic ? product.MakerArabicName : product.MakerEnglishName,
                IsAvailable = product.StockQuantity > 0,
                Image = new ProductMainImageViewModel
                {
                    Id = productMainImage.Id,
                    HasBackgroundColor = imageHasBackgroundColor,
                    ColorCode = finalColor,
                    OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                    MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                    SmallImage = _appImageHelper.GenerateImageModel(smallImage),
                    MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                }
            };
        }

        public async Task<List<ProductImageViewModel>> GetProductImagesViewModelsAsync(string id, string categoryId, string subCategoryId, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductsRepository.FindAsync(id, cancellationToken);
            
            var productImages = await _unitOfWork.ProductImagesRepository.GetImagesWithImageModelsAsync(id, cancellationToken);
            var parentCategory = await _unitOfWork.CategoriesRepository.FindAsync(categoryId, cancellationToken);
            var subCategory = await _unitOfWork.CategoriesRepository.FindAsync(subCategoryId, cancellationToken);

            var isCategoryColorSetAndValid = parentCategory != null && parentCategory.ColorType == ColorType.Custom && parentCategory.ColorCode.IsHtmlColor();
            var categoryColorCode = parentCategory?.ColorCode;

            var isSubCategoryColorSetAndValid = subCategory != null && subCategory.ColorType == ColorType.Custom && subCategory.ColorCode.IsHtmlColor();
            var subCategoryColorCode = subCategory?.ColorCode;

            var finalColor = string.Empty;
            var imageHasBackgroundColor = false;

            var productImagesViewModels = new List<ProductImageViewModel>();

            foreach (var image in productImages)
            {
                if (product.ColorType == ColorType.Custom)
                {
                    finalColor = product.ColorCode;
                    imageHasBackgroundColor = true;
                }
                else if (product.ColorType == ColorType.FromParent)
                {
                    if (isSubCategoryColorSetAndValid)
                    {
                        finalColor = subCategoryColorCode;
                        imageHasBackgroundColor = true;
                    }
                    else if (subCategory != null && subCategory.ColorType == ColorType.FromParent && isCategoryColorSetAndValid)
                    {
                        finalColor = categoryColorCode;
                        imageHasBackgroundColor = true;
                    }
                }

                var croppedImage = image.OriginalImage;
                var mediumImage = image.MediumImage;
                var smallImage = image.SmallImage;
                var mobileImage = image.MobileImage;

                productImagesViewModels.Add(new ProductImageViewModel
                {
                    Id = image.Id,
                    HasBackgroundColor = imageHasBackgroundColor,
                    ColorCode = finalColor,
                    OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                    MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                    SmallImage = _appImageHelper.GenerateImageModel(smallImage),
                    MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                });
            }

            return productImagesViewModels;
        }

        public async Task<List<ProductStateViewModel>> GetProductStatesViewModelsAsync(string id, CancellationToken cancellationToken)
        {
            var states = await _unitOfWork.ProductStatesRepository.GetStatesForProductAsync(id, cancellationToken);

            return states.Select(s => new ProductStateViewModel
            {
                Text = _loggedInUser.IsCultureArabic ? s.ArabicName : s.EnglishName
            }).ToList();
        }

        public async Task<List<SimilarProductViewModel>> GetSimilarProductsViewModelsAsync(string id, CancellationToken cancellationToken)
        {
            var similarProducts = new List<SimilarProductViewModel>();

            var randomThreeProducts = await _unitOfWork.ProductsRepository.Table.OrderBy(p => p.Number)
                .Where(p => p.Id != id && p.IsVisible && p.ProductCategories.Any(pc => 
                    _unitOfWork.ProductCategoriesRepository.Table.Where(pc1 => pc1.ProductId == id)
                        .Select(pc1 => pc1.CategoryId).Contains(pc.CategoryId)))
                .OrderBy(p => Guid.NewGuid()).Take(3).ToListAsync(cancellationToken);

            var randomProductsIds = randomThreeProducts.Select(p => p.Id);

            var randomNotInSameCategoryProduct = await _unitOfWork.ProductCategoriesRepository.Table
                .Where(pc => pc.Product.IsVisible && !randomProductsIds.Contains(pc.ProductId) && !_unitOfWork.ProductCategoriesRepository.Table.Where(pc1 => pc.ProductId == id)
                    .Select(pc1 => pc1.CategoryId).Contains(pc.CategoryId))
                .OrderBy(p => Guid.NewGuid()).Select(p => p.Product)
                .FirstOrDefaultAsync(cancellationToken);

            foreach (var product in randomThreeProducts)
            {
                var image = await _unitOfWork.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(product.Id, cancellationToken);

                var croppedImage = image.OriginalImage;
                var mediumImage = image.MediumImage;
                var smallImage = image.SmallImage;
                var mobileImage = image.MobileImage;

                similarProducts.Add(new SimilarProductViewModel
                {
                    Id = product.Id,
                    Name = _loggedInUser.IsCultureArabic ? product.ArabicName : product.EnglishName,
                    MainImage = new ProductMainImageViewModel
                    {
                        ColorCode = product.ColorCode,
                        HasBackgroundColor = product.ColorType == ColorType.Custom && product.ColorCode.IsHtmlColor(),
                        OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                        MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                        SmallImage = _appImageHelper.GenerateImageModel(smallImage),
                        MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                    }
                });
            }

            if (randomNotInSameCategoryProduct != null)
            {
                var image = await _unitOfWork.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(randomNotInSameCategoryProduct.Id, cancellationToken);

                var croppedImage = image.OriginalImage;
                var mediumImage = image.MediumImage;
                var smallImage = image.SmallImage;
                var mobileImage = image.MobileImage;

                similarProducts.Add(new SimilarProductViewModel
                {
                    Id = randomNotInSameCategoryProduct.Id,
                    Name = _loggedInUser.IsCultureArabic ? randomNotInSameCategoryProduct.ArabicName : randomNotInSameCategoryProduct.EnglishName,
                    MainImage = new ProductMainImageViewModel
                    {
                        ColorCode = randomNotInSameCategoryProduct.ColorCode,
                        HasBackgroundColor = randomNotInSameCategoryProduct.ColorType == ColorType.Custom && randomNotInSameCategoryProduct.ColorCode.IsHtmlColor(),
                        OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                        MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                        SmallImage = _appImageHelper.GenerateImageModel(smallImage),
                        MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                    }
                });
            }

            return similarProducts;
        }

        public async Task<PagedViewModel<ProductViewModel>> SearchProductsAsync(GetSearchProductsOptions options, CancellationToken cancellationToken)
        {
            var productsQuery = _unitOfWork.ProductsRepository.Table.Where(p => p.IsVisible).OrderBy(p => p.Number).AsQueryable();

            if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
                productsQuery = productsQuery.Where(p => p.ArabicName.ToLower().Contains(options.Text.ToLower()) || p.EnglishName.ToLower().Contains(options.Text.ToLower()));

            productsQuery = productsQuery.Distinct();

            var total = await  productsQuery.CountAsync(cancellationToken);
            var products = await productsQuery.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize).ToListAsync(cancellationToken);
            var productsModels = new List<ProductViewModel>();

            var productsIds = products.Select(p => p.Id);

            var productsImages = await _unitOfWork.ProductMainImagesRepository.GetProductsImagesGroupedByIdAsync(productsIds, cancellationToken);

            foreach (var product in products)
            {
                var productMainImage = productsImages[product.Id];

                var croppedImage = productMainImage.OriginalImage;
                var mediumImage = productMainImage.MediumImage;
                var smallImage = productMainImage.SmallImage;
                var mobileImage = productMainImage.MobileImage;

                productsModels.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Number = product.Number,
                    Age = product.Age,
                    Dimensions = _loggedInUser.IsCultureArabic ? product.DimensionsInArabic : product.DimensionsInEnglish,
                    Description = _loggedInUser.IsCultureArabic ? product.ArabicDescription : product.EnglishDescription,
                    Name = _loggedInUser.IsCultureArabic ? product.ArabicName : product.EnglishName,
                    Maker = _loggedInUser.IsCultureArabic ? product.MakerArabicName : product.MakerEnglishName,
                    IsAvailable = product.StockQuantity > 0,
                    Image = new ProductMainImageViewModel
                    {
                        Id = productMainImage.Id,
                        HasBackgroundColor = product.ColorType == ColorType.Custom && product.ColorCode.IsHtmlColor(),
                        ColorCode = product.ColorCode,
                        OriginalImage = _appImageHelper.GenerateImageModel(croppedImage),
                        MediumImage = _appImageHelper.GenerateImageModel(mediumImage),
                        SmallImage = _appImageHelper.GenerateImageModel(smallImage),
                        MobileImage = _appImageHelper.GenerateImageModel(mobileImage)
                    }
                });
            }

            return productsModels.AsPagedModel(options.CurrentPage, options.PageSize, total);
        }
    }
}
