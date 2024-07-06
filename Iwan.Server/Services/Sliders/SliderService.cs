using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Media;
using Iwan.Server.Domain.Sliders;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sliders
{
    [Injected(ServiceLifetime.Scoped, typeof(ISliderService))]
    public class SliderService : ISliderService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IQuerySettingService _querySettingService;
        protected readonly IImageService _imageService;
        protected readonly IFileProvider _fileProvider;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public SliderService(IFileProvider fileProvider, IImageService imageService, IUnitOfWork unitOfWork,
            IQuerySettingService querySettingService, ILoggedInUserProvider loggedInUserProvider)
        {
            _fileProvider = fileProvider;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _querySettingService = querySettingService;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<SliderImage> AddSliderImageAsync(AddSliderImageDto imageDto, CancellationToken cancellationToken = default)
        {
            var tempImageExist = await _unitOfWork.TempImagesRepository.ExistAsync(imageDto.Image.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            // Get images settings
            var imagesSettings = await _querySettingService.GetSlidersImagesSettingsAsync(cancellationToken);

            // Virtual path to slider image
            var virtualPathToSliderImages = _fileProvider.Combine(AppDirectories.Images.SELF, AppDirectories.Images.Slider);
            
            var croppedTempImage = await _unitOfWork.TempImagesRepository.FindAsync(imageDto.Image.Id, cancellationToken);

            var croppedImageExtension = _fileProvider.GetFileExtension(croppedTempImage.FileName);

            // Form cropped image full path and filename
            (var croppedImageFullPath, var croppedImageFileName) = _fileProvider.FormNewFilePath(virtualPathToSliderImages, ImagesPostfixes.SliderCropped, croppedImageExtension);

            var addedCroppedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
            {
                FileName = croppedImageFileName,
                VirtualPath = virtualPathToSliderImages,
                MimeType = croppedImageExtension.ToMimeType()
            }, cancellationToken);

            // Form cropped temp image full path
            var croppedTempImageFullPath = _fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.FileName);

            var addedMediumImage = await _imageService.ResizeAndAddVersionOfImageAsync(croppedTempImageFullPath, virtualPathToSliderImages, croppedImageExtension, ImagesPostfixes.SliderMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, true, cancellationToken);

            var addedMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(croppedTempImageFullPath, virtualPathToSliderImages, croppedImageExtension, ImagesPostfixes.SliderMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, true, cancellationToken);

            // Add the slider image as a model
            var sliderImage = await AddSliderImageAsync(imageDto.Order, addedCroppedImage.Id, addedMediumImage.Id, addedMobileImage.Id, cancellationToken);

            _fileProvider.MoveFile(croppedTempImageFullPath, croppedImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(croppedTempImage.VirtualPath, croppedTempImage.SmallVersionFileName));
            _unitOfWork.TempImagesRepository.Delete(croppedTempImage);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            // Return the new slider image
            return sliderImage;
        }

        private async Task<SliderImage> AddSliderImageAsync(int order, string croppedImageId, string mediumImageId, string mobileImageId, CancellationToken cancellationToken = default)
        {
            var sliderImage = new SliderImage
            {
                Order = order,
                OriginalImageId = croppedImageId,
                MediumImageId = mediumImageId,
                MobileImageId = mobileImageId
            };

            await _unitOfWork.SliderImagesRepository.AddAsync(sliderImage, cancellationToken);
            return sliderImage;
        }

        public async Task DeleteSliderImageAsync(string sliderImageId, bool deleteImageFiles = true, CancellationToken cancellationToken = default)
        {
            var sliderImageQuery = _unitOfWork.SliderImagesRepository.Table;

            if (deleteImageFiles)
            {
                sliderImageQuery = sliderImageQuery
                    .Include(i => i.OriginalImage).Include(i => i.MediumImage).Include(i => i.MobileImage);
            }

            var sliderImage = await sliderImageQuery.SingleOrDefaultAsync(i => i.Id == sliderImageId, cancellationToken);

            var images = new Image[] { sliderImage.OriginalImage, sliderImage.MediumImage, sliderImage.MobileImage };
            if (deleteImageFiles)
                _imageService.DeleteImagesFiles(images);

            _unitOfWork.ImagesRepository.DeleteRange(images);

            _unitOfWork.SliderImagesRepository.Delete(sliderImage);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task<SliderImage> EditSliderImageAsync(EditSliderImageDto editedImage, CancellationToken cancellationToken)
        {
            var slider = await _unitOfWork.SliderImagesRepository.FindAsync(editedImage.Id, cancellationToken);

            if (slider == null)
                throw new NotFoundException(Responses.Sliders.SliderImageNotFound);

            slider.Order = editedImage.Order;

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return slider;
        }
    }
}
