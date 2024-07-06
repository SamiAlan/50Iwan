using Iwan.Server.Services.Media;
using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Pages;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Iwan.Server.Services.Settings;
using Iwan.Server.Domain.Media;
using Iwan.Shared.Extensions;

namespace Iwan.Server.Services.Pages
{
    [Injected(ServiceLifetime.Scoped, typeof(IPagesService))]
    public class PagesService : IPagesService
    {
        protected readonly IUnitOfWork _context;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;
        protected readonly IImageService _imageService;
        protected readonly IFileProvider _fileProvider;
        protected readonly IPagesQueryService _homeQueryService;
        protected readonly IQuerySettingService _querySettingService;

        public PagesService(IUnitOfWork unitOfWork, IFileProvider fileProvider,
            ILoggedInUserProvider loggedInUserProvider, IImageService imageService,
            IPagesQueryService homeQueryService, IQuerySettingService querySettingService)
        {
            _context = unitOfWork;
            _loggedInUserProvider = loggedInUserProvider;
            _imageService = imageService;
            _fileProvider = fileProvider;
            _homeQueryService = homeQueryService;
            _querySettingService = querySettingService;
        }

        public async Task<Color> AddColorAsync(AddColorDto color, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetColorPickingSectionAsync(cancellationToken);

            var addedColor = await _context.ColorsRepository.AddAsync(new Color
            {
                ColorCode = color.ColorCode,
                SectionId = section.Id
            }, cancellationToken);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return addedColor;
        }

        public async Task<AboutUsSectionImage> AddImageAsync(AddAboutUsSectionImageDto imageDto, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetAboutUsSectionAsync(cancellationToken);

            var tempImageExist = await _context.TempImagesRepository.ExistAsync(imageDto.Image.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            // Get images settings
            var imagesSettings = await _querySettingService.GetAboutUsSectionImagesSettingsAsync(cancellationToken);

            // Virtual path to about us image
            var virtualPathToAboutUsImages = _fileProvider.Combine(AppDirectories.Images.SELF, AppDirectories.Images.AboutUs);

            var croppedTempImage = await _context.TempImagesRepository.FindAsync(imageDto.Image.Id, cancellationToken);

            var croppedImageExtension = _fileProvider.GetFileExtension(croppedTempImage.FileName);

            // Form cropped image full path and filename
            (var croppedImageFullPath, var croppedImageFileName) = _fileProvider.FormNewFilePath(virtualPathToAboutUsImages, ImagesPostfixes.AboutUsCropped, croppedImageExtension);

            var addedCroppedImage = await _context.ImagesRepository.AddAsync(new Image
            {
                FileName = croppedImageFileName,
                VirtualPath = virtualPathToAboutUsImages,
                MimeType = croppedImageExtension.ToMimeType()
            }, cancellationToken);

            // Form cropped temp image full path
            var croppedTempImageFullPath = _fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.FileName);

            var addedMediumImage = await _imageService.ResizeAndAddVersionOfImageAsync(croppedTempImageFullPath, virtualPathToAboutUsImages, croppedImageExtension, ImagesPostfixes.AboutUsMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, true, cancellationToken);

            var addedMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(croppedTempImageFullPath, virtualPathToAboutUsImages, croppedImageExtension, ImagesPostfixes.AboutUsMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, true, cancellationToken);

            // Add the slider image as a model
            var aboutUsImage = await AddAboutUsImageAsync(section.Id, addedCroppedImage.Id, addedMediumImage.Id, addedMobileImage.Id, cancellationToken);

            _fileProvider.MoveFile(croppedTempImageFullPath, croppedImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.SmallVersionFileName));
            _context.TempImagesRepository.Delete(croppedTempImage);

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            // Return the new about us image
            return aboutUsImage;
        }

        public async Task<InteriorDesignSectionImage> ChangeMainImageAsync(ChangeInteriorDesignSectionMainImageDto image, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetInteriorDesignSectionAsync(cancellationToken);

            // Check if temp images already exist
            var tempImageExist = await _context.TempImagesRepository.ExistAsync(image.MainImage.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var currentImage = await _context.InteriorDesignSectionImagesRepository.GetInteriorImageAsync(section.Id, cancellationToken);

            var newMainImage = await GenerateAndAddInteriorDesignSectionMainImageAsync(image.MainImage.Id, cancellationToken);

            if (currentImage == null)
            {
                currentImage = new InteriorDesignSectionImage
                {
                    InteriorDesignSectionId = section.Id
                };

                await _context.InteriorDesignSectionImagesRepository.AddAsync(currentImage, cancellationToken);
            }
            else
            {
                if (!currentImage.MainImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    _imageService.DeleteImageFile(currentImage.MainImage);
                    _context.ImagesRepository.Delete(currentImage.MainImage);
                }
            }

            currentImage.MainImageId = newMainImage.Id;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return currentImage;
        }

        public async Task<InteriorDesignSectionImage> ChangeMobileImageAsync(ChangeInteriorDesignSectionMobileImageDto image, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetInteriorDesignSectionAsync(cancellationToken);

            // Check if temp images already exist
            var tempImageExist = await _context.TempImagesRepository.ExistAsync(image.MobileImage.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var currentImage = await _context.InteriorDesignSectionImagesRepository.GetInteriorImageAsync(section.Id, cancellationToken);

            var newMobileImage = await GenerateAndAddInteriorDesignSectionMobileImageAsync(image.MobileImage.Id, cancellationToken);

            if (currentImage == null)
            {
                currentImage = new InteriorDesignSectionImage
                {
                    InteriorDesignSectionId = section.Id
                };

                await _context.InteriorDesignSectionImagesRepository.AddAsync(currentImage, cancellationToken);
            }
            else
            {
                if (!currentImage.MobileImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    _imageService.DeleteImageFile(currentImage.MobileImage);
                    _context.ImagesRepository.Delete(currentImage.MobileImage);
                }
            }

            currentImage.MobileImageId = newMobileImage.Id;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return currentImage;
        }

        public async Task DeleteAboutUsImageAsync(string aboutUsImageId, CancellationToken cancellationToken = default)
        {
            var image = await _context.AboutUsSectionImagesRepository.GetIncludingImagesAsync(aboutUsImageId, cancellationToken);

            if (image == null)
                throw new NotFoundException(Responses.Images.ImageNotFound);

            var imagesToDelete = new[] { image.OriginalImage, image.MediumImage, image.MobileImage };
            _imageService.DeleteImagesFiles(imagesToDelete);
            _context.ImagesRepository.DeleteRange(imagesToDelete);
            _context.AboutUsSectionImagesRepository.Delete(image);
            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task DeleteColorAsync(string colorId, CancellationToken cancellationToken = default)
        {
            var color = await _context.ColorsRepository.FindAsync(colorId, cancellationToken);

            if (color == null)
                throw new NotFoundException(Responses.General.ColorNotFound);

            _context.ColorsRepository.Delete(color);
            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task<HeaderSection> EditSectionAsync(EditHeaderSectionDto header, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetHeaderSectionAsync(cancellationToken);

            section.ArabicTitle = header.ArabicTitle;
            section.ArabicSubtitle1 = header.ArabicSubtitle1;
            section.ArabicSubtitle2 = header.ArabicSubtitle2;
            section.EnglishTitle = header.EnglishTitle;
            section.EnglishSubtitle1 = header.EnglishSubtitle1;
            section.EnglishSubtitle2 = header.EnglishSubtitle2;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return section;
        }

        public async Task<ContactUsSection> EditSectionAsync(EditContactUsSectionDto contactUs, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetContactUsSectionAsync(cancellationToken);

            section.Email = contactUs.Email;
            section.Location = contactUs.Location;
            section.PhoneNumber = contactUs.PhoneNumber;
            section.FacebookUrl = contactUs.FacebookUrl;
            section.InstagramUrl = contactUs.InstagramUrl;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return section;
        }

        public async Task<AboutUsSection> EditSectionAsync(EditAboutUsSectionDto aboutUs, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetAboutUsSectionAsync(cancellationToken);

            section.EnglishText = aboutUs.EnglishText;
            section.ArabicText = aboutUs.ArabicText;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return section;
        }

        public async Task<ServicesSection> EditSectionAsync(EditServicesSectionDto services, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetServicesSectionAsync(cancellationToken);

            section.Service1ArabicText = services.Service1ArabicText;
            section.Service2ArabicText = services.Service2ArabicText;
            section.Service1ArabicTitle = services.Service1ArabicTitle;
            section.Service2ArabicTitle = services.Service2ArabicTitle;
            section.Service1EnglishText = services.Service1EnglishText;
            section.Service2EnglishText = services.Service2EnglishText;
            section.Service1EnglishTitle = services.Service1EnglishTitle;
            section.Service2EnglishTitle = services.Service2EnglishTitle;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return section;
        }

        public async Task<InteriorDesignSection> EditSectionAsync(EditInteriorDesignSectionDto interiorDesign, CancellationToken cancellationToken = default)
        {
            var section = await _homeQueryService.GetInteriorDesignSectionAsync(cancellationToken);

            section.ArabicText = interiorDesign.ArabicText;
            section.EnglishText = interiorDesign.EnglishText;
            section.Url = interiorDesign.Url;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return section;
        }

        private async Task<AboutUsSectionImage> AddAboutUsImageAsync(string aboutUsSectionId, string croppedImageId, string mediumImageId, string mobileImageId, CancellationToken cancellationToken = default)
        {
            var sliderImage = new AboutUsSectionImage
            {
                AboutUsSectionId = aboutUsSectionId,
                OriginalImageId = croppedImageId,
                MediumImageId = mediumImageId,
                MobileImageId = mobileImageId
            };

            await _context.AboutUsSectionImagesRepository.AddAsync(sliderImage, cancellationToken);
            return sliderImage;
        }

        private async Task<Image> GenerateAndAddInteriorDesignSectionMainImageAsync(string mainTempImageId, CancellationToken cancellationToken = default)
        {
            var mobileTempImage = await _context.TempImagesRepository.FindAsync(mainTempImageId, cancellationToken);

            var mainImageExtension = _fileProvider.GetFileExtension(mobileTempImage.FileName);

            // Virtual path to interior design images directory
            var virtualPathToInteriorDesignSectionImages = _fileProvider.Combine(AppDirectories.Images.SELF,
                AppDirectories.Images.InteriorDesign);

            // Form main image full path and filename
            (var mainImageFullPath, var mainImageFileName) = _fileProvider.FormNewFilePath(virtualPathToInteriorDesignSectionImages, ImagesPostfixes.InteriorDesignMain, mainImageExtension);

            var addedMainImage = await _context.ImagesRepository.AddAsync(new Image
            {
                FileName = mainImageFileName,
                VirtualPath = virtualPathToInteriorDesignSectionImages,
                MimeType = mainImageExtension.ToMimeType()
            }, cancellationToken);

            // Form main temp image full path
            var mainTempImageFullPath = _fileProvider.CombineWithRoot(mobileTempImage.VirtualPath, mobileTempImage.FileName);

            _fileProvider.MoveFile(mainTempImageFullPath, mainImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(mobileTempImage.VirtualPath, mobileTempImage.SmallVersionFileName));

            _context.TempImagesRepository.Delete(mobileTempImage);

            return addedMainImage;
        }
        
        private async Task<Image> GenerateAndAddInteriorDesignSectionMobileImageAsync(string mobileTTempImageId, CancellationToken cancellationToken = default)
        {
            var mobileTempImage = await _context.TempImagesRepository.FindAsync(mobileTTempImageId, cancellationToken);

            var mobileImageExtension = _fileProvider.GetFileExtension(mobileTempImage.FileName);

            // Virtual path to interior design images directory
            var virtualPathToInteriorDesignSectionImages = _fileProvider.Combine(AppDirectories.Images.SELF,
                AppDirectories.Images.InteriorDesign);

            // Form mobile image full path and filename
            (var mobileImageFullPath, var mobileImageFileName) = _fileProvider.FormNewFilePath(virtualPathToInteriorDesignSectionImages, ImagesPostfixes.InteriorDesignMobile, mobileImageExtension);

            var addedMainImage = await _context.ImagesRepository.AddAsync(new Image
            {
                FileName = mobileImageFileName,
                VirtualPath = virtualPathToInteriorDesignSectionImages,
                MimeType = mobileImageExtension.ToMimeType()
            }, cancellationToken);

            // Form mobile temp image full path
            var mobileTempImageFullPath = _fileProvider.CombineWithRoot(mobileTempImage.VirtualPath, mobileTempImage.FileName);

            _fileProvider.MoveFile(mobileTempImageFullPath, mobileImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(mobileTempImage.VirtualPath, mobileTempImage.SmallVersionFileName));

            _context.TempImagesRepository.Delete(mobileTempImage);

            return addedMainImage;
        }
    }
}
