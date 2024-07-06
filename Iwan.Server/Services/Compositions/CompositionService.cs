using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Common;
using Iwan.Server.Domain.Compositions;
using Iwan.Server.Domain.Media;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Compositions
{
    [Injected(ServiceLifetime.Scoped, typeof(ICompositionService))]
    public class CompositionService : ICompositionService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IFileProvider _fileProvider;
        protected readonly IImageService _imageService;
        protected readonly IQuerySettingService _querySettingService;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public CompositionService(IUnitOfWork unitOfWork, IFileProvider fileProvider,
            IImageService imageService, IQuerySettingService querySettingService,
            ILoggedInUserProvider loggedInUserProvider)
        {
            _unitOfWork = unitOfWork;
            _fileProvider = fileProvider;
            _imageService = imageService;
            _querySettingService = querySettingService;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<Composition> AddCompositionAsync(AddCompositionDto compositionToAdd, CancellationToken cancellationToken)
        {
            var compositionWithSameName = await _unitOfWork.CompositionsRepository
                    .FirstOrDefaultAsync(c => c.ArabicName.ToLower() == compositionToAdd.ArabicName.ToLower() || c.EnglishName.ToLower() == compositionToAdd.EnglishName.ToLower(), cancellationToken);

            if (compositionWithSameName != null)
                throw new BadRequestException(Responses.Compositions.CompositionSameNameAlreadyExist,
                    compositionToAdd.ArabicName.EqualsIgnoreCase(compositionWithSameName.ArabicName) ?
                        compositionToAdd.GetPropertyPath(c => c.ArabicName) : compositionToAdd.GetPropertyPath(c => c.EnglishName));

            // Check if temp images already exist
            var tempImageExist = await _unitOfWork.TempImagesRepository
                .ExistAsync(compositionToAdd.Image.Image.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound,
                    compositionToAdd.GetPropertyPath(c => c.Image));

            var composition = new Composition
            {
                ArabicName = compositionToAdd.ArabicName,
                EnglishName = compositionToAdd.EnglishName,
                ArabicDescription = compositionToAdd.ArabicDescription,
                EnglishDescription = compositionToAdd.EnglishDescription,
                IsVisible = compositionToAdd.IsVisible
            };

            if (compositionToAdd.ColorTypeId.EqualsEnum(ColorType.Custom))
            {
                composition.ColorType = ColorType.Custom;
                composition.ColorCode = compositionToAdd.ColorCode;
            }
            else if (compositionToAdd.ColorTypeId.EqualsEnum(ColorType.FromParent))
            {
                composition.ColorType = ColorType.FromParent;
            }
            else
                composition.ColorType = ColorType.NoChange;

            await GenerateAndAddCompositionImageAsync(composition.Id, compositionToAdd.Image.Image.Id, cancellationToken);

            await _unitOfWork.CompositionsRepository.AddAsync(composition, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

            return composition;
        }

        public async Task<CompositionImage> ChangeCompositionImageAsync(string compositionId, ChangeCompositionImageDto compositionImage, CancellationToken cancellationToken)
        {
            var composition = await _unitOfWork.CompositionsRepository.FindAsync(compositionId, cancellationToken);

            if (composition == null)
                throw new NotFoundException(Responses.Compositions.CompositionNotExist);

            // Check if temp images already exist
            var tempImageExist = await _unitOfWork.TempImagesRepository.ExistAsync(compositionImage.Image.Id, cancellationToken);

            if (!tempImageExist)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            var currentImage = await _unitOfWork.CompositionImagesRepository.GetByCompositionIdAsync(composition.Id, cancellationToken);

            var newImage = await GenerateAndAddCompositionImageAsync(composition.Id, compositionImage.Image.Id, cancellationToken);

            if (currentImage != null)
            {
                var images = new Image[] { currentImage.OriginalImage, currentImage.MediumImage,
                    currentImage.MobileImage };

                _imageService.DeleteImagesFiles(images);
                _unitOfWork.ImagesRepository.DeleteRange(images);
                _unitOfWork.CompositionImagesRepository.Delete(currentImage);
            }

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return newImage;
        }

        public async Task DeleteCompositionAsync(string compositionId, CancellationToken cancellationToken)
        {
            var composition = await _unitOfWork.CompositionsRepository.FindAsync(compositionId, cancellationToken);

            if (composition is null)
                throw new NotFoundException(Responses.Compositions.CompositionNotExist);

            var currentImage = await _unitOfWork.CompositionImagesRepository.GetByCompositionIdAsync(composition.Id, cancellationToken);

            if (currentImage != null)
            {
                var images = new Image[]
                { currentImage.OriginalImage, currentImage.MediumImage,
                  currentImage.MobileImage };

                _imageService.DeleteImagesFiles(images);
                _unitOfWork.ImagesRepository.DeleteRange(images);
                _unitOfWork.CompositionImagesRepository.Delete(currentImage);

            }

            _unitOfWork.CompositionsRepository.Delete(composition);

            await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<Composition> EditCompositionAsync(EditCompositionDto editedComposition, CancellationToken
            cancellationToken)
        {
            var composition = await _unitOfWork.CompositionsRepository.FindAsync(editedComposition.Id, cancellationToken);

            if (composition is null)
                throw new NotFoundException(Responses.Compositions.CompositionNotExist);

            var compositionWithSameName = await _unitOfWork.CompositionsRepository.FirstOrDefaultAsync
                (c => c.ArabicName.ToLower() == editedComposition.ArabicName.ToLower() || c.EnglishName.ToLower() == editedComposition.EnglishName.ToLower() && c.Id != composition.Id, cancellationToken);

            if (compositionWithSameName is not null)
                throw new AlreadyExistException(Responses.Compositions.CompositionSameNameAlreadyExist,
                    composition.ArabicName.EqualsIgnoreCase(compositionWithSameName.ArabicName) ?
                        composition.GetPropertyPath(c => c.ArabicName) : composition.GetPropertyPath(c => c.EnglishName));

            composition.SetIfNotEqual(c => c.ArabicName, editedComposition.ArabicName);
            composition.SetIfNotEqual(c => c.EnglishName, editedComposition.EnglishName);
            composition.SetIfNotEqual(c => c.ArabicDescription, editedComposition.ArabicDescription);
            composition.SetIfNotEqual(c => c.EnglishDescription, editedComposition.EnglishDescription);
            composition.SetIfNotEqual(c => c.IsVisible, editedComposition.IsVisible);

            if (editedComposition.ColorTypeId.EqualsEnum(ColorType.Custom))
            {
                composition.ColorType = ColorType.Custom;
                composition.ColorCode = composition.ColorCode;
            }
            else if (editedComposition.ColorTypeId.EqualsEnum(ColorType.FromParent))
            {
                composition.ColorType = ColorType.FromParent;
            }
            else
                composition.ColorType = ColorType.NoChange;

            await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

            return composition;
        }

        #region Helpers

        private async Task<CompositionImage> GenerateAndAddCompositionImageAsync(string compositionId, string backgroundlessCroppedTempImageId, CancellationToken cancellationToken = default)
        {
            var backgroundlessCroppedTempImage = await _unitOfWork.TempImagesRepository.FindAsync(backgroundlessCroppedTempImageId, cancellationToken);

            var backgroundlessCroppedImageExtension = _fileProvider.GetFileExtension(backgroundlessCroppedTempImage.FileName);

            // Get images settings
            var imagesSettings = await _querySettingService.GetCompositionsImagesSettingsAsync(cancellationToken);

            // Virtual path to compositions images directory
            var virtualPathToCompositionsImages = _fileProvider.Combine(AppDirectories.Images.SELF,
                AppDirectories.Images.Compositions);

            // Form backgroundless cropped image full path and filename
            (var backgroundlessCroppedImageFullPath, var backgroundlessCroppedImageFileName) = _fileProvider.FormNewFilePath(virtualPathToCompositionsImages, ImagesPostfixes.CompositionBackgroundlessCropped, backgroundlessCroppedImageExtension);

            var addedBackgroundlessCroppedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
            {
                FileName = backgroundlessCroppedImageFileName,
                VirtualPath = virtualPathToCompositionsImages,
                MimeType = backgroundlessCroppedImageExtension.ToMimeType()
            }, cancellationToken);

            // Form backgroundless cropped temp image full path
            var backgroundlessCroppedTempImageFullPath = _fileProvider.CombineWithRoot(backgroundlessCroppedTempImage.VirtualPath, backgroundlessCroppedTempImage.FileName);

            var addedBackgroundlessMediumImage = await _imageService.ResizeAndAddVersionOfImageAsync(backgroundlessCroppedTempImageFullPath, virtualPathToCompositionsImages, backgroundlessCroppedImageExtension, ImagesPostfixes.CompositionBackgroundlessMedium, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, true, cancellationToken);

            //var addedMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(croppedTempImageFullPath, virtualPathToCompositionsImages, croppedImageExtension, ImagesPostfixes.CompositionMobile, imagesSettings.CompositionImageMobileWidth, imagesSettings.CompositionImageMobileHeight, cancellationToken);
            var addedBackgroundlessMobileImage = await _imageService.ResizeAndAddVersionOfImageAsync(backgroundlessCroppedTempImageFullPath, virtualPathToCompositionsImages, backgroundlessCroppedImageExtension, ImagesPostfixes.CompositionBackgroundlessMobile, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, true, cancellationToken);

            // set images ids to the composition
            var compositionImage = new CompositionImage
            {
                CompositionId = compositionId,
                OriginalImage = addedBackgroundlessCroppedImage,
                MediumImage = addedBackgroundlessMediumImage,
                MobileImage = addedBackgroundlessMobileImage,
            };

            await _unitOfWork.CompositionImagesRepository.AddAsync(compositionImage, cancellationToken);

            _fileProvider.MoveFile(backgroundlessCroppedTempImageFullPath, backgroundlessCroppedImageFullPath, true);
            _fileProvider.DeleteFile(_fileProvider.CombineWithRoot(backgroundlessCroppedTempImage.VirtualPath, backgroundlessCroppedTempImage.FileName));

            _unitOfWork.TempImagesRepository.Delete(backgroundlessCroppedTempImage);

            return compositionImage;
        }

        #endregion
    }
}
