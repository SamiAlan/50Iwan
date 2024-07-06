using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Settings;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Settings
{
    [Injected(ServiceLifetime.Scoped, typeof(IQuerySettingService))]
    public class QuerySettingService : IQuerySettingService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;
        protected readonly IAppImageHelper _appImageHelper;

        public QuerySettingService(IUnitOfWork unitOfWork, ILoggedInUserProvider loggedInUserProvider,
            IAppImageHelper appImageHelper)
        {
            _unitOfWork = unitOfWork;
            _loggedInUserProvider = loggedInUserProvider;
            _appImageHelper = appImageHelper;
        }

        public async Task<ProductsImagesSettings> GetProductsImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.ProductsImagesSettingsRepository.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new ProductsImagesSettings();
                await _unitOfWork.ProductsImagesSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<CategoriesImagesSettings> GetCategoriesImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.CategoriesImagesSettingsRepository.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new CategoriesImagesSettings();
                await _unitOfWork.CategoriesImagesSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<CompositionsImagesSettings> GetCompositionsImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.CompositionsImagesSettingsRepository.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new CompositionsImagesSettings();
                await _unitOfWork.CompositionsImagesSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<AboutUsSectionImagesSettings> GetAboutUsSectionImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.AboutUsSectionImagesSettingsRepository.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new AboutUsSectionImagesSettings();
                await _unitOfWork.AboutUsSectionImagesSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<SlidersImagesSettings> GetSlidersImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.SlidersImagesSettingsRepository.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new SlidersImagesSettings();
                await _unitOfWork.SlidersImagesSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<TempImagesSettings> GetTempImagesSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.TempImagesSettingsRepository.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new TempImagesSettings();
                await _unitOfWork.TempImagesSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<TempImagesSettingsDto> GetTempImagesSettingsDetailsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await GetTempImagesSettingsAsync(cancellationToken);

            return new TempImagesSettingsDto
            {
                TempImagesExpirationDays = settings.TempImagesExpirationDays,
                DelayInMinutes = settings.DelayInMinutes
            };
        }

        public async Task<ImagesSettingsDto> GetImagesSettingsDetailsAsync(CancellationToken cancellationToken = default)
        {
            var productsSettings = await GetProductsImagesSettingsAsync(cancellationToken);
            var categoriesSettings = await GetCategoriesImagesSettingsAsync(cancellationToken);
            var slidersSettings = await GetSlidersImagesSettingsAsync(cancellationToken);
            var compositionsSettings = await GetCompositionsImagesSettingsAsync(cancellationToken);
            var aboutUsSectionSettings = await GetAboutUsSectionImagesSettingsAsync(cancellationToken);

            return new ImagesSettingsDto
            {
                ProductMediumImageWidth = productsSettings.MediumImageWidth,
                ProductMediumImageHeight = productsSettings.MediumImageHeight,
                ProductSmallImageWidth = productsSettings.SmallImageWidth,
                ProductSmallImageHeight = productsSettings.SmallImageHeight,
                ProductMobileImageWidth = productsSettings.MobileImageWidth,
                ProductMobileImageHeight = productsSettings.MobileImageHeight,

                CategoryMediumImageWidth = categoriesSettings.MediumImageWidth,
                CategoryMediumImageHeight = categoriesSettings.MediumImageHeight,
                CategoryMobileImageWidth = categoriesSettings.MobileImageWidth,
                CategoryMobileImageHeight = categoriesSettings.MobileImageHeight,

                CompositionMediumImageWidth = compositionsSettings.MediumImageWidth,
                CompositionMediumImageHeight = compositionsSettings.MediumImageHeight,
                CompositionMobileImageWidth = compositionsSettings.MobileImageWidth,
                CompositionMobileImageHeight = compositionsSettings.MobileImageHeight,

                AboutUsSectionMediumImageWidth = aboutUsSectionSettings.MediumImageWidth,
                AboutUsSectionMediumImageHeight = aboutUsSectionSettings.MediumImageHeight,
                AboutUsSectionMobileImageWidth = aboutUsSectionSettings.MobileImageWidth,
                AboutUsSectionMobileImageHeight = aboutUsSectionSettings.MobileImageHeight,

                SliderImageMediumWidth = slidersSettings.MediumImageWidth,
                SliderImageMediumHeight = slidersSettings.MediumImageHeight,
                SliderImageMobileWidth = slidersSettings.MobileImageWidth,
                SliderImageMobileHeight = slidersSettings.MobileImageHeight
            };
        }

        public async Task<WatermarkSettings> GetWatermarkImageSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.WatermarkSettingsRepository.GetWithImageAsync(cancellationToken);

            if (settings == null)
            {
                settings = new WatermarkSettings();
                await _unitOfWork.WatermarkSettingsRepository.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            }

            return settings;
        }

        public async Task<WatermarkSettingsDto> GetWatermarkImageSettingsDetailsAsync(CancellationToken cancellationToken)
        {
            var settings = await GetWatermarkImageSettingsAsync(cancellationToken);
            var dto = new WatermarkSettingsDto
            {
                ShouldAddWatermark = settings.ShouldAddWatermark,
                Opacity = settings.Opacity
            };

            if (settings.WatermarkImage == null) return dto;

            dto.WatermarkImage = new WatermarkImageDto
            {
                Image = _appImageHelper.GenerateImageDto(settings.WatermarkImage)
            };

            return dto;
        }
    }
}
