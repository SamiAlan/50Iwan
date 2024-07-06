using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Options.Catalog;
using Iwan.Shared.Options.Compositions;
using Iwan.Shared.Options.Vendors;
using Iwan.Shared.Options.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Options.SliderImages;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Options.Accounts;
using Iwan.Shared.Extensions;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Dtos.Pages;

namespace Iwan.Client.Blazor.Extensions
{
    public static class MappingsExtensions
    {
        public static EditVendorDto MapToEditVendorDto(this VendorDto vendor)
        {
            return new EditVendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                BenefitPercent = vendor.BenefitPercent,
                ShowOwnProducts = vendor.ShowOwnProducts
            };
        }

        public static EditAddressDto MapToEditAddressDto(this AddressDto address)
        {
            return new EditAddressDto
            {
                Id = address.Id,
                City = address.City,
                Company = address.Company,
                Country = address.Country,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Email = address.Email,
                PhoneNumber = address.PhoneNumber
            };
        }

        public static EditCategoryDto MapToEditCategoryDto(this CategoryDto category)
        {
            return new EditCategoryDto
            {
                Id = category.Id,
                ArabicName = category.ArabicName,
                EnglishName = category.EnglishName,
                ColorCode = category.ColorCode,
                ColorTypeId = category.ColorTypeId,
                IsVisible = category.IsVisible,
                ParentCategoryId = category.ParentCategoryId
            };
        }

        public static EditCompositionDto MapToEditCompositionDto(this CompositionDto composition)
        {
            return new EditCompositionDto
            {
                Id = composition.Id,
                ArabicName = composition.ArabicName,
                EnglishName = composition.EnglishName,
                ArabicDescription = composition.ArabicDescription,
                EnglishDescription = composition.EnglishDescription,
                ColorCode = composition.ColorCode,
                IsVisible = composition.IsVisible
            };
        }

        public static CategoriesImagesSettingsDto MapToCategoriesSettingsDto(this ImagesSettingsDto settings)
        {
            return new CategoriesImagesSettingsDto
            {
                MediumImageWidth = settings.CategoryMediumImageWidth,
                MediumImageHeight = settings.CategoryMediumImageHeight,
                MobileImageHeight = settings.CategoryMobileImageHeight,
                MobileImageWidth = settings.CategoryMobileImageWidth
            };
        }

        public static CompositionsImagesSettingsDto MapToCompositionsSettingsDto(this ImagesSettingsDto settings)
        {
            return new CompositionsImagesSettingsDto
            {
                MediumImageWidth = settings.CompositionMediumImageWidth,
                MediumImageHeight = settings.CompositionMediumImageHeight,
                MobileImageHeight = settings.CompositionMobileImageHeight,
                MobileImageWidth = settings.CompositionMobileImageWidth,
            };
        }

        public static ProductsImagesSettingsDto MapToProductsSettingsDto(this ImagesSettingsDto settings)
        {
            return new ProductsImagesSettingsDto
            {
                MediumImageWidth = settings.ProductMediumImageWidth,
                MediumImageHeight = settings.ProductMediumImageHeight,
                SmallImageHeight = settings.ProductSmallImageHeight,
                SmallImageWidth = settings.ProductSmallImageWidth,
                MobileImageHeight = settings.ProductMobileImageHeight,
                MobileImageWidth = settings.ProductMobileImageWidth,
            };
        }

        public static SlidersImagesSettingsDto MapToSlidersImagesSettingsDto(this ImagesSettingsDto settings)
        {
            return new SlidersImagesSettingsDto
            {
                MediumImageWidth = settings.SliderImageMediumWidth,
                MediumImageHeight = settings.SliderImageMediumHeight,
                MobileImageHeight = settings.SliderImageMobileHeight,
                MobileImageWidth = settings.SliderImageMobileWidth,
            };
        }

        public static AboutUsSectionImagesSettingsDto MapToAboutUsSectionImagesSettingsDto(this ImagesSettingsDto settings)
        {
            return new AboutUsSectionImagesSettingsDto
            {
                MediumImageWidth = settings.AboutUsSectionMediumImageWidth,
                MediumImageHeight = settings.AboutUsSectionMediumImageHeight,
                MobileImageWidth = settings.AboutUsSectionMobileImageWidth,
                MobileImageHeight = settings.AboutUsSectionMobileImageHeight,
            };
        }

        public static EditProductDto MapToEditProductDto(this ProductDto product)
        {
            return new EditProductDto
            {
                Id = product.Id,
                Number = product.Number,
                ArabicName = product.ArabicName,
                EnglishName = product.EnglishName,
                ArabicDescription = product.ArabicDescription,
                EnglishDescription = product.EnglishDescription,
                MakerEnglishName = product.MakerEnglishName,
                MakerArabicName = product.MakerArabicName,
                Age = product.Age,
                DimensionsInEnglish = product.DimensionsInEnglish,
                DimensionsInArabic = product.DimensionsInArabic,
                ColorCode = product.ColorCode,
                ColorTypeId = product.ColorTypeId,
                IsVisible = product.IsVisible,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                VendorId = product.Vendor?.Id,
            };
        }

        public static EditSliderImageDto MapToEditSliderImageDto(this SliderImageDto sliderImage)
        {
            return new EditSliderImageDto
            {
                Id = sliderImage.Id,
                Order = sliderImage.Order,
            };
        }

        public static UpdateProfileDto MapToUpdateProfileDto(this UserDto user)
        {
            return new UpdateProfileDto
            {
                Name = user.Name
            };
        }

        public static EditHeaderSectionDto MapToEditHeaderSectionDto(this HeaderSectionDto header)
        {
            return new EditHeaderSectionDto
            {
                ArabicTitle = header.ArabicTitle,
                ArabicSubtitle1 = header.ArabicSubtitle1,
                ArabicSubtitle2 = header.ArabicSubtitle2,
                EnglishTitle = header.EnglishTitle,
                EnglishSubtitle1 = header.EnglishSubtitle1,
                EnglishSubtitle2 = header.EnglishSubtitle2
            };
        }

        public static EditContactUsSectionDto MapToEditContactUsSectionDto(this ContactUsSectionDto contactUs)
        {
            Console.WriteLine(contactUs.Email);
            return new EditContactUsSectionDto
            {
                Location = contactUs.Location,
                Email = contactUs.Email,
                PhoneNumber = contactUs.PhoneNumber,
                FacebookUrl = contactUs.FacebookUrl,
                InstagramUrl = contactUs.InstagramUrl
            };
        }

        public static EditServicesSectionDto MapToEditServicesSectionDto(this ServicesSectionDto services)
        {
            return new EditServicesSectionDto
            {
                Service1ArabicText = services.Service1ArabicText,
                Service1ArabicTitle = services.Service1ArabicTitle,
                Service2ArabicText = services.Service2ArabicText,
                Service2ArabicTitle = services.Service2ArabicTitle,
                Service1EnglishText = services.Service1EnglishText,
                Service1EnglishTitle = services.Service1EnglishTitle,
                Service2EnglishText = services.Service2EnglishText,
                Service2EnglishTitle = services.Service2EnglishTitle
            };
        }

        public static EditAboutUsSectionDto MapToEditAboutUsSectionDto(this AboutUsSectionDto aboutUs)
        {
            return new EditAboutUsSectionDto
            {
                ArabicText = aboutUs.ArabicText,
                EnglishText = aboutUs.EnglishText
            };
        }

        public static EditWatermarkSettingsDto ToEditDto(this WatermarkSettingsDto settings)
        {
            return new EditWatermarkSettingsDto
            {
                ShouldAddWatermark = settings.ShouldAddWatermark,
                Opacity = settings.Opacity,
            };
        }

        public static EditInteriorDesignSectionDto MapToEditInteriorDesignSectionDto(this InteriorDesignSectionDto interiorDesign)
        {
            return new EditInteriorDesignSectionDto
            {
                ArabicText = interiorDesign.ArabicText,
                EnglishText = interiorDesign.EnglishText,
                Url = interiorDesign.Url
            };
        }

        public static string ToQueryStringParameters(this GetVendorsOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            items.Add(ValueTuple.Create(nameof(GetVendorsOptions.CurrentPage), options.CurrentPage.ToString()));
            items.Add(ValueTuple.Create(nameof(GetVendorsOptions.PageSize), options.PageSize.ToString()));

            if (!string.IsNullOrEmpty(options.Name))
                items.Add(ValueTuple.Create(nameof(GetVendorsOptions.Name), options.Name));

            if (options.OnlyVendorsShowingTheirProducts.HasValue)
                items.Add(ValueTuple.Create(nameof(GetVendorsOptions.OnlyVendorsShowingTheirProducts), options.OnlyVendorsShowingTheirProducts.Value.ToString()));

            if (options.MinBenefitPercentage.HasValue)
                items.Add(ValueTuple.Create(nameof(GetVendorsOptions.MinBenefitPercentage), options.MinBenefitPercentage.Value.ToString()));

            if (options.MaxBenefitPercentage.HasValue)
                items.Add(ValueTuple.Create(nameof(GetVendorsOptions.MaxBenefitPercentage), options.MaxBenefitPercentage.Value.ToString()));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }

        public static string ToQueryStringParameters(this GetAllCategoriesOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            if (!string.IsNullOrEmpty(options.Text))
                items.Add(ValueTuple.Create(nameof(GetAllCategoriesOptions.Text), options.Text));

            if (!string.IsNullOrEmpty(options.UnderParentCategoryId))
                items.Add(ValueTuple.Create(nameof(GetAllCategoriesOptions.UnderParentCategoryId), options.UnderParentCategoryId));

            if (options.OnlyHasRelatedProducts.HasValue)
                items.Add(ValueTuple.Create(nameof(GetAllCategoriesOptions.OnlyHasRelatedProducts), options.OnlyHasRelatedProducts.Value.ToString()));

            if (options.OnlyVisible.HasValue)
                items.Add(ValueTuple.Create(nameof(GetAllCategoriesOptions.OnlyVisible), options.OnlyVisible.Value.ToString()));

            items.Add(ValueTuple.Create(nameof(GetAllCategoriesOptions.IncludeImages), options.IncludeImages.ToString()));

            if (options.Type.HasValue)
                items.Add(ValueTuple.Create(nameof(GetAllCategoriesOptions.Type), options.Type.Value.ToString()));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }

        public static string ToQueryStringParameters(this GetCategoriesOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.CurrentPage), options.CurrentPage.ToString()));
            items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.PageSize), options.PageSize.ToString()));

            if (!string.IsNullOrEmpty(options.Text))
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.Text), options.Text));

            if (!string.IsNullOrEmpty(options.UnderParentCategoryId))
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.UnderParentCategoryId), options.UnderParentCategoryId));

            if (options.OnlyHasRelatedProducts.HasValue)
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.OnlyHasRelatedProducts), options.OnlyHasRelatedProducts.Value.ToString()));

            if (options.OnlyVisible.HasValue)
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.OnlyVisible), options.OnlyVisible.Value.ToString()));

            items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.IncludeImages), options.IncludeImages.ToString()));

            if (options.Type.HasValue)
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.Type), options.Type.Value.ToString()));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }

        public static string ToQueryStringParameters(this GetCompositionsOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            items.Add(ValueTuple.Create(nameof(GetCompositionsOptions.CurrentPage), options.CurrentPage.ToString()));
            items.Add(ValueTuple.Create(nameof(GetCompositionsOptions.PageSize), options.PageSize.ToString()));

            if (!string.IsNullOrEmpty(options.Text))
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.Text), options.Text));

            if (options.OnlyVisible.HasValue)
                items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.OnlyVisible), options.OnlyVisible.Value.ToString()));

            items.Add(ValueTuple.Create(nameof(GetCategoriesOptions.IncludeImages), options.IncludeImages.ToString()));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }

        public static string ToQueryStringParameters(this GetSliderImagesOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            items.Add(ValueTuple.Create(nameof(GetSliderImagesOptions.CurrentPage), options.CurrentPage.ToString()));
            items.Add(ValueTuple.Create(nameof(GetSliderImagesOptions.PageSize), options.PageSize.ToString()));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }

        public static string ToQueryStringParameters(this GetUsersOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            items.Add(ValueTuple.Create(nameof(GetUsersOptions.CurrentPage), options.CurrentPage.ToString()));
            items.Add(ValueTuple.Create(nameof(GetUsersOptions.PageSize), options.PageSize.ToString()));

            if (!options.Name.IsNullOrEmptyOrWhiteSpaceSafe())
                items.Add(ValueTuple.Create(nameof(GetUsersOptions.Name), options.Name));

            if (!options.Email.IsNullOrEmptyOrWhiteSpaceSafe())
                items.Add(ValueTuple.Create(nameof(GetUsersOptions.Email), options.Email));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }

        public static string ToQueryStringParameters(this AdminGetProductsOptions options)
        {
            var items = new List<(string, string)>();
            var queryStrings = string.Empty;

            items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.CurrentPage), options.CurrentPage.ToString()));
            items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.PageSize), options.PageSize.ToString()));

            if (!options.Text.IsNullOrEmptyOrWhiteSpaceSafe())
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.Text), options.Text));

            if (options.OnlyVisible.HasValue)
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.OnlyVisible), options.OnlyVisible.Value.ToString()));

            if (options.Number.HasValue)
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.Number), options.Number.Value.ToString()));

            if (options.HavingNoMainImage.HasValue)
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.HavingNoMainImage), options.HavingNoMainImage.Value.ToString()));

            if (options.OnlyNeedingResize.HasValue)
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.OnlyNeedingResize), options.OnlyNeedingResize.Value.ToString()));

            if (options.OnlyUnattached.HasValue)
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.OnlyUnattached), options.OnlyUnattached.Value.ToString()));

            if (options.OnlyOwnedProducts.HasValue)
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.OnlyOwnedProducts), options.OnlyOwnedProducts.Value.ToString()));

            if (!options.UnderCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.UnderCategoryId), options.UnderCategoryId));

            if (!options.UnderSubCategoryId.IsNullOrEmptyOrWhiteSpaceSafe())
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.UnderSubCategoryId), options.UnderSubCategoryId));

            if (!options.UnderVendorId.IsNullOrEmptyOrWhiteSpaceSafe())
                items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.UnderVendorId), options.UnderVendorId));

            items.Add(ValueTuple.Create(nameof(AdminGetProductsOptions.IncludeMainImage), options.IncludeMainImage.ToString()));

            if (items.Any())
            {
                queryStrings = $"{items[0].Item1}={items[0].Item2}";

                foreach (var tuple in items.Skip(1))
                {
                    queryStrings += $"&{tuple.Item1}={tuple.Item2}";
                }
            }

            return queryStrings;
        }
    }
}
