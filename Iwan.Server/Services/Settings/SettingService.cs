using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Settings;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Media;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Settings
{
    [Injected(ServiceLifetime.Scoped, typeof(ISettingService))]
    public class SettingService : ISettingService
    {
        protected readonly IUnitOfWork _context;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;
        protected readonly IQuerySettingService _querySettingsService;
        protected readonly IFileProvider _fileProvider;
        protected readonly IAppImageHelper _appImageHelper;
        protected readonly IImageService _imageService;

        public SettingService(IUnitOfWork context, ILoggedInUserProvider loggedInUserProvider,
            IQuerySettingService querySettingsService, IFileProvider fileProvider,
            IAppImageHelper appImageHelper, IImageService imageService)
        {
            _context = context;
            _loggedInUserProvider = loggedInUserProvider;
            _querySettingsService = querySettingsService;
            _fileProvider = fileProvider;
            _appImageHelper = appImageHelper;
            _imageService = imageService;
        }

        public async Task<TempImagesSettings> UpdateSettingsAsync(TempImagesSettingsDto settingsDto, CancellationToken cancellationToken)
        {
            var settings = await _querySettingsService.GetTempImagesSettingsAsync(cancellationToken);

            settings.SetIfNotEqual(s => s.DelayInMinutes, settingsDto.DelayInMinutes);
            settings.SetIfNotEqual(s => s.TempImagesExpirationDays, settingsDto.TempImagesExpirationDays);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
            
            return settings;
        }

        public async Task<CategoriesImagesSettingsDto> UpdateSettingsAsync(CategoriesImagesSettingsDto settingsDto, CancellationToken cancellationToken)
        {
            var settings = await _querySettingsService.GetCategoriesImagesSettingsAsync(cancellationToken);
            
            settings.SetIfNotEqual(s => s.MediumImageWidth, settingsDto.MediumImageWidth);
            settings.SetIfNotEqual(s => s.MediumImageHeight, settingsDto.MediumImageHeight);
            settings.SetIfNotEqual(s => s.MobileImageWidth, settingsDto.MobileImageWidth);
            settings.SetIfNotEqual(s => s.MobileImageHeight, settingsDto.MobileImageHeight);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return settingsDto;
        }

        public async Task<CompositionsImagesSettingsDto> UpdateSettingsAsync(CompositionsImagesSettingsDto settingsDto, CancellationToken cancellationToken)
        {
            var settings = await _querySettingsService.GetCompositionsImagesSettingsAsync(cancellationToken);
            
            settings.SetIfNotEqual(s => s.MediumImageWidth, settingsDto.MediumImageWidth);
            settings.SetIfNotEqual(s => s.MediumImageHeight, settingsDto.MediumImageHeight);
            settings.SetIfNotEqual(s => s.MobileImageWidth, settingsDto.MobileImageWidth);
            settings.SetIfNotEqual(s => s.MobileImageHeight, settingsDto.MobileImageHeight);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return settingsDto;
        }

        public async Task<AboutUsSectionImagesSettingsDto> UpdateSettingsAsync(AboutUsSectionImagesSettingsDto settingsDto, CancellationToken cancellationToken)
        {
            var settings = await _querySettingsService.GetAboutUsSectionImagesSettingsAsync(cancellationToken);
            
            settings.SetIfNotEqual(s => s.MediumImageWidth, settingsDto.MediumImageWidth);
            settings.SetIfNotEqual(s => s.MediumImageHeight, settingsDto.MediumImageHeight);
            settings.SetIfNotEqual(s => s.MobileImageWidth, settingsDto.MobileImageWidth);
            settings.SetIfNotEqual(s => s.MobileImageHeight, settingsDto.MobileImageHeight);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return settingsDto;
        }
        
        public async Task<ProductsImagesSettingsDto> UpdateSettingsAsync(ProductsImagesSettingsDto settingsDto, CancellationToken cancellationToken)
        {
            var settings = await _querySettingsService.GetProductsImagesSettingsAsync(cancellationToken);
            
            settings.SetIfNotEqual(s => s.MediumImageWidth, settingsDto.MediumImageWidth);
            settings.SetIfNotEqual(s => s.MediumImageHeight, settingsDto.MediumImageHeight);
            settings.SetIfNotEqual(s => s.SmallImageWidth, settingsDto.SmallImageWidth);
            settings.SetIfNotEqual(s => s.SmallImageHeight, settingsDto.SmallImageHeight);
            settings.SetIfNotEqual(s => s.MobileImageWidth, settingsDto.MobileImageWidth);
            settings.SetIfNotEqual(s => s.MobileImageHeight, settingsDto.MobileImageHeight);
            settings.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return settingsDto;
        }

        public async Task<SlidersImagesSettingsDto> UpdateSettingsAsync(SlidersImagesSettingsDto settingsDto, CancellationToken cancellationToken)
        {
            var settings = await _querySettingsService.GetSlidersImagesSettingsAsync(cancellationToken);
            
            settings.SetIfNotEqual(s => s.MediumImageWidth, settingsDto.MediumImageWidth);
            settings.SetIfNotEqual(s => s.MediumImageHeight, settingsDto.MediumImageHeight);
            settings.SetIfNotEqual(s => s.MobileImageWidth, settingsDto.MobileImageWidth);
            settings.SetIfNotEqual(s => s.MobileImageHeight, settingsDto.MobileImageHeight);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return settingsDto;
        }

        public async Task<WatermarkSettings> UpdateSettingsAsync(EditWatermarkSettingsDto newSettings, CancellationToken cancellationToken = default)
        {
            var settings = await _querySettingsService.GetWatermarkImageSettingsAsync(cancellationToken);

            if (newSettings.ShouldAddWatermark && settings.WatermarkImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                throw new BadRequestException(Responses.Settings.CantWatermarkWithoutImage,
                    newSettings.GetPropertyPath(s => s.ShouldAddWatermark));

            settings.ShouldAddWatermark = newSettings.ShouldAddWatermark;
            settings.Opacity = newSettings.Opacity;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return settings;
        }

        public async Task<WatermarkImageDto> ChangeWatermarkImageAsync(ChangeWatermarkImageDto newImage, CancellationToken cancellationToken = default)
        {
            var settings = await _querySettingsService.GetWatermarkImageSettingsAsync(cancellationToken);

            var tempImage = await _context.TempImagesRepository.FindAsync(newImage.Image.Id, cancellationToken);

            if (tempImage == null)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var imageExtension = _fileProvider.GetFileExtension(tempImage.FileName);
            var tempOriginalImageFullPath = _fileProvider.CombineWithRoot(tempImage.VirtualPath, tempImage.FileName);
            var tempSmallImageFullPath = _fileProvider.CombineWithRoot(tempImage.VirtualPath, tempImage.SmallVersionFileName);
            var virtualPathToWatermarkFolder = _fileProvider.Combine(AppDirectories.Images.SELF, AppDirectories.Images.Watermark);

            (var newWatermarkImageFullPath, var watermarkImageFileName) = _fileProvider.FormNewFilePath(virtualPathToWatermarkFolder, "", imageExtension);

            var addedWatermarkImage = await _context.ImagesRepository.AddAsync(new Domain.Media.Image
            {
                FileName = watermarkImageFileName,
                MimeType = imageExtension.ToMimeType(),
                VirtualPath = virtualPathToWatermarkFolder
            }, cancellationToken);

            if (!settings.WatermarkImageId.IsNullOrEmptyOrWhiteSpaceSafe())
            {
                _imageService.DeleteImageFile(settings.WatermarkImage);
                _context.ImagesRepository.Delete(settings.WatermarkImage);
            }

            settings.WatermarkImageId = addedWatermarkImage.Id;

            _fileProvider.MoveFile(tempOriginalImageFullPath, newWatermarkImageFullPath);
            _fileProvider.DeleteFile(tempSmallImageFullPath);
            _context.TempImagesRepository.Delete(tempImage);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return new WatermarkImageDto
            {
                Image = _appImageHelper.GenerateImageDto(addedWatermarkImage)
            };
        }
    }
}
