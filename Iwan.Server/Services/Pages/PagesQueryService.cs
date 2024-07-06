using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Pages;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Models.Common;
using Iwan.Server.Models.Pages;
using Iwan.Server.Options;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Catalog;
using Iwan.Server.Services.Products;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Pages
{
    [Injected(ServiceLifetime.Scoped, typeof(IPagesQueryService))]
    public class PagesQueryService : IPagesQueryService
    {
        protected readonly IUnitOfWork _context;
        protected IAppImageHelper _appImageHelper;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;
        protected readonly IQueryCategoryService _queryCategoryService;
        protected readonly IQueryProductService _queryProductService;

        public PagesQueryService(IUnitOfWork context, IAppImageHelper appImageHelper,
            ILoggedInUserProvider loggedInUserProvider, IQueryCategoryService queryCategoryService,
            IQueryProductService queryProductService)
        {
            _context = context;
            _appImageHelper = appImageHelper;
            _loggedInUserProvider = loggedInUserProvider;
            _queryCategoryService = queryCategoryService;
            _queryProductService = queryProductService;
        }

        public async Task<AboutUsSection> GetAboutUsSectionAsync(CancellationToken cancellationToken = default)
        {
            var aboutUsSection = await _context.AboutUsSectionsRepository.FirstOrDefaultAsync(cancellationToken);

            if (aboutUsSection == null)
            {
                aboutUsSection = new();
                await _context.AboutUsSectionsRepository.AddAsync(aboutUsSection, cancellationToken);
                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return aboutUsSection;
        }

        public async Task<AboutUsSectionDto> GetAboutUsSectionDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetAboutUsSectionAsync(cancellationToken);

            var images = await _context.AboutUsSectionImagesRepository.GetImagesSectionAsync(cancellationToken);

            return new AboutUsSectionDto
            {
                ArabicText = section.ArabicText,
                EnglishText = section.EnglishText,
                Images = images.Select(i => new AboutUsSectionImageDto
                {
                    Id = i.Id,
                    OriginalImage = _appImageHelper.GenerateImageDto(i.OriginalImage),
                    MediumImage = _appImageHelper.GenerateImageDto(i.MediumImage),
                    MobileImage = _appImageHelper.GenerateImageDto(i.MobileImage),
                }).ToList()
            };
        }

        public async Task<AboutUsSectionImageDto> GetAboutUsSectionImageDetailsAsync(string imageId, CancellationToken cancellationToken = default)
        {
            var image = await _context.AboutUsSectionImagesRepository.GetIncludingImagesAsync(imageId, cancellationToken);

            if (image == null)
                throw new NotFoundException(Responses.About.ImageNotFound);

            return new AboutUsSectionImageDto
            {
                Id = image.Id,
                OriginalImage = _appImageHelper.GenerateImageDto(image.OriginalImage),
                MediumImage = _appImageHelper.GenerateImageDto(image.MediumImage),
                MobileImage = _appImageHelper.GenerateImageDto(image.MobileImage)
            };
        }

        public async Task<IEnumerable<AboutUsSectionImageDto>> GetAboutUsSectionImagesDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetAboutUsSectionAsync(cancellationToken);

            var images = await _context.AboutUsSectionImagesRepository.GetImagesSectionAsync(cancellationToken);

            return images.Select(i => new AboutUsSectionImageDto
            {
                Id = i.Id,
                OriginalImage = _appImageHelper.GenerateImageDto(i.OriginalImage),
                MediumImage = _appImageHelper.GenerateImageDto(i.MediumImage),
                MobileImage = _appImageHelper.GenerateImageDto(i.MobileImage),
            }).ToList();
        }

        public async Task<ContactUsSection> GetContactUsSectionAsync(CancellationToken cancellationToken = default)
        {
            var section = await _context.ContactUsSectionsRepository.FirstOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                section = new();
                await _context.ContactUsSectionsRepository.AddAsync(section, cancellationToken);
                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return section;
        }

        public async Task<ContactUsSectionDto> GetContactUsDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetContactUsSectionAsync(cancellationToken);

            return new ContactUsSectionDto
            {
                Email = section.Email,
                Location = section.Location,
                PhoneNumber = section.PhoneNumber,
                FacebookUrl = section.FacebookUrl,
                InstagramUrl = section.InstagramUrl
            };
        }

        public async Task<HeaderSection> GetHeaderSectionAsync(CancellationToken cancellationToken = default)
        {
            var section = await _context.HeaderSectionsRepository.FirstOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                section = new();
                await _context.HeaderSectionsRepository.AddAsync(section, cancellationToken);
                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return section;
        }

        public async Task<HeaderSectionDto> GetHeaderSectionDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetHeaderSectionAsync(cancellationToken);

            return new HeaderSectionDto
            {
                ArabicTitle = section.ArabicTitle,
                ArabicSubtitle1 = section.ArabicSubtitle1,
                ArabicSubtitle2 = section.ArabicSubtitle2,
                EnglishTitle = section.EnglishTitle,
                EnglishSubtitle1 = section.EnglishSubtitle1,
                EnglishSubtitle2 = section.EnglishSubtitle2
            };
        }

        public async Task<HomePageContentDto> GetHomePageContentDetailsAsync(CancellationToken cancellationToken = default)
        {
            return new HomePageContentDto
            {
                AboutUsSection = await GetAboutUsSectionDetailsAsync(cancellationToken),
                ContactUsSection = await GetContactUsDetailsAsync(cancellationToken),
                HeaderSection = await GetHeaderSectionDetailsAsync(cancellationToken),
                ServicesSection = await GetServicesSectionDetailsAsync(cancellationToken),
                InteriorDesignSection = await GetInteriorDesignSectionDetailsAsync(cancellationToken)
            };
        }

        public async Task<InteriorDesignSection> GetInteriorDesignSectionAsync(CancellationToken cancellationToken = default)
        {
            var section = await _context.InteriorDesignSectionsRepository.FirstOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                section = new();
                await _context.InteriorDesignSectionsRepository.AddAsync(section, cancellationToken);
                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return section;
        }

        public async Task<InteriorDesignSectionDto> GetInteriorDesignSectionDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetInteriorDesignSectionAsync(cancellationToken);

            var image = await _context.InteriorDesignSectionImagesRepository.GetInteriorImageAsync(section.Id, cancellationToken);

            var sectionDto = new InteriorDesignSectionDto
            {
                ArabicText = section.ArabicText,
                EnglishText = section.EnglishText,
                Url = section.Url
            };

            if (image == null) return sectionDto;

            sectionDto.Image = new InteriorDesignSectionImageDto
            {
                Id = image.Id
            };

            if (!image.MainImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                sectionDto.Image.MainImage = _appImageHelper.GenerateImageDto(image.MainImage);

            if (!image.MobileImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                sectionDto.Image.MobileImage = _appImageHelper.GenerateImageDto(image.MobileImage);

            return sectionDto;
        }

        public async Task<InteriorDesignSectionImageDto> GetInteriorDesignSectionImageDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetInteriorDesignSectionAsync(cancellationToken);

            var image = await _context.InteriorDesignSectionImagesRepository.GetInteriorImageAsync(section.Id, cancellationToken);

            var dto = new InteriorDesignSectionImageDto
            {
                Id = image.Id
            };

            if (!image.MainImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                dto.MainImage = _appImageHelper.GenerateImageDto(image.MainImage);

            if (!image.MobileImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                dto.MobileImage = _appImageHelper.GenerateImageDto(image.MobileImage);

            return dto;
        }

        public async Task<ServicesSection> GetServicesSectionAsync(CancellationToken cancellationToken = default)
        {
            var section = await _context.ServicesSectionsRepository.FirstOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                section = new();
                await _context.ServicesSectionsRepository.AddAsync(section, cancellationToken);
                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return section;
        }

        public async Task<ServicesSectionDto> GetServicesSectionDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetServicesSectionAsync(cancellationToken);

            return new ServicesSectionDto
            {
                Service1ArabicText = section.Service1ArabicText,
                Service2ArabicText = section.Service2ArabicText,
                Service1ArabicTitle = section.Service1ArabicTitle,
                Service2ArabicTitle = section.Service2ArabicTitle,
                Service1EnglishText = section.Service1EnglishText,
                Service2EnglishText = section.Service2EnglishText,
                Service1EnglishTitle = section.Service1EnglishTitle,
                Service2EnglishTitle = section.Service2EnglishTitle,
            };
        }

        public async Task<ProductDetailsPageContentDto> GetProductDetailsContentPageAsync(CancellationToken cancellationToken = default)
        {
            return new ProductDetailsPageContentDto
            {
                ColorPickingSection = await GetColorPickingSectionDetailsAsync(cancellationToken)
            };
        }

        public async Task<ColorPickingSectionDto> GetColorPickingSectionDetailsAsync(CancellationToken cancellationToken = default)
        {
            var section = await GetColorPickingSectionAsync(cancellationToken);

            return new ColorPickingSectionDto
            {
                Id = section.Id,
                Colors = section.Colors.Select(c => new ColorDto
                {
                    Id = c.Id,
                    ColorCode = c.ColorCode
                }).ToList()
            };
        }

        public async Task<ColorPickingSection> GetColorPickingSectionAsync(CancellationToken cancellationToken = default)
        {
            var section = await _context.ColorPickingSectionsRepository.FirstOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                section = new();
                await _context.ColorPickingSectionsRepository.AddAsync(section, cancellationToken);
                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return section;
        }

        public async Task<ProductsPageContentViewModel> GetProductsPageContentAsync(GetProductsPageOptions options, CancellationToken cancellationToken)
        {
            var subCategoriesResponse = await _queryCategoryService.GetSubCategoriesForPublicAsync(options.CategoryId, cancellationToken);

            if (subCategoriesResponse.StatusCode != HttpStatusCode.OK)
                return null;

            var pageContent = new ProductsPageContentViewModel
            {
                IsCultureArabic = _loggedInUserProvider.IsCultureArabic,
                SelectedCategoryId = options.CategoryId,
                SelectedSubCategoryId = options.SubCategoryId,
                SubCategories = subCategoriesResponse.Data.ToList(),
                SelectedCategoryName = await _queryCategoryService.GetLocalizedCategoryNameAsync(options.CategoryId, cancellationToken),
                Products = await _queryProductService.GetProductsAsync(options, cancellationToken)
            };

            return pageContent;
        }

        public async Task<ProductDetailsPageContentViewModel> GetProductDetailsPageContentAsync(string productId, string categoryId, CancellationToken cancellationToken)
        {
            var product = await _context.ProductsRepository.FindAsync(productId, cancellationToken);

            if (product == null) return null;
            if (!product.IsVisible) return null;

            var contactUsInfo = await GetContactUsDetailsAsync(cancellationToken);

            var pageContent = new ProductDetailsPageContentViewModel
            {
                RecepientEmail = contactUsInfo.Email,
                PhoneNumber = contactUsInfo.PhoneNumber,
                IsCultureArabic = _loggedInUserProvider.IsCultureArabic
            };

            var category = await _context.CategoriesRepository.FindAsync(categoryId, cancellationToken);

            if (category != null)
            {
                if (!category.IsSubCategory)
                {
                    pageContent.SelectedCategoryId = category.Id;
                    pageContent.SelectedCategoryName = await _queryCategoryService.GetLocalizedCategoryNameAsync(category.Id, cancellationToken);
                }
                else
                {
                    pageContent.SelectedSubCategoryId = category.Id;
                    pageContent.SelectedSubCategoryName = await _queryCategoryService.GetLocalizedCategoryNameAsync(category.Id, cancellationToken);
                    pageContent.SelectedCategoryId = category.ParentCategoryId;
                    pageContent.SelectedCategoryName = await _queryCategoryService.GetLocalizedCategoryNameAsync(category.ParentCategoryId, cancellationToken);
                }
            }

            pageContent.Product = await _queryProductService.GetProductDetailsViewModelAsync(product.Id, pageContent.SelectedCategoryId, pageContent.SelectedSubCategoryId, cancellationToken);

            pageContent.Images = await _queryProductService.GetProductImagesViewModelsAsync(product.Id, pageContent.SelectedCategoryId, pageContent.SelectedSubCategoryId, cancellationToken);
            
            pageContent.States = await _queryProductService.GetProductStatesViewModelsAsync(product.Id, cancellationToken);

            pageContent.Colors = (await GetColorPickingSectionAsync(cancellationToken)).Colors.Select(c => new ColorViewModel
            {
                ColorCode = c.ColorCode
            }).ToList();

            pageContent.SimilarProducts = await _queryProductService.GetSimilarProductsViewModelsAsync(product.Id, cancellationToken);

            return pageContent;
        }

        public async Task<SearchPageContentViewModel> GetSearchPageContentAsync(GetSearchProductsOptions options, CancellationToken cancellationToken)
        {
            var pageContent = new SearchPageContentViewModel
            {
                IsCultureArabic = _loggedInUserProvider.IsCultureArabic,
                Text = options.Text?.Trim(),
            };

            pageContent.Products = await _queryProductService.SearchProductsAsync(options, cancellationToken);

            return pageContent;
        }
    }
}
