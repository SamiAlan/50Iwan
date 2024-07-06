using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Media;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Media
{
    [Injected(ServiceLifetime.Scoped, typeof(IImageService))]
    public class ImageService : IImageService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IFileProvider _fileProvider;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;
        protected readonly IImageManipulatorService _imageResizerService;
        protected readonly IQuerySettingService _querySettingService;

        public ImageService(IUnitOfWork unitOfWork, IFileProvider fileProvider,
            ILoggedInUserProvider loggedInUserProvider, IImageManipulatorService imageResizerService, 
            IQuerySettingService querySettingService)
        {
            _unitOfWork = unitOfWork;
            _fileProvider = fileProvider;
            _loggedInUserProvider = loggedInUserProvider;
            _imageResizerService = imageResizerService;
            _querySettingService = querySettingService;
        }

        public async Task<TempImage> UploadTempImageAsync(IFormFile imageFile, bool useTXFileManager = true, CancellationToken cancellationToken = default)
        {
            var stream = imageFile.OpenReadStream();
            return await UploadTempImageAsync(stream, imageFile.FileName, imageFile.Length, imageFile.ContentType, useTXFileManager, cancellationToken);
        }

        public async Task<TempImage> UploadTempImageAsync(Stream readStream, string filename, long length, string contentType, bool useTXFileManager = true, CancellationToken cancellationToken = default)
        {
            var virtualPathToTempImagesFolder = _fileProvider.Combine(AppDirectories.Images.SELF, AppDirectories.Images.Temp);
            (var fullPath, var fileName) = _fileProvider.FormNewFilePath(virtualPathToTempImagesFolder, ImagesPostfixes.Temp,
                _fileProvider.GetFileExtension(filename));

            (var smallVersionFullPath, var smallVersionFileName) = _fileProvider.FormNewFilePath(virtualPathToTempImagesFolder, ImagesPostfixes.TempSmall,
                _fileProvider.GetFileExtension(fileName));

            using (readStream)
            {
                await _fileProvider.WriteAllBytesAsync(readStream, length, fullPath, useTXFileManager, cancellationToken);
            }

            var smallVersionBytes = await _imageResizerService.ResizeAndGetBytesAsync(fullPath, 200, 200, cancellationToken);

            await _fileProvider.WriteAllBytesAsync(smallVersionFullPath, smallVersionBytes, useTXFileManager, cancellationToken);

            var settings = await _querySettingService.GetTempImagesSettingsAsync(cancellationToken);

            var tempImage = new TempImage
            {
                MimeType = contentType,
                FileName = fileName,
                SmallVersionFileName = smallVersionFileName,
                VirtualPath = virtualPathToTempImagesFolder,
                ExpirationDate = DateTime.UtcNow.AddDays(settings.TempImagesExpirationDays)
            };

            await _unitOfWork.TempImagesRepository.AddAsync(tempImage, cancellationToken);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return tempImage;
        }

        public async Task DeleteTempImageAsync(string tempImageId, bool deleteImageFile = true, bool useTXFileManager = true, CancellationToken cancellationToken = default)
        {
            var tempImage = await _unitOfWork.TempImagesRepository.FindAsync(tempImageId, cancellationToken);

            if (tempImage == null)
                throw new NotFoundException(Responses.Images.TempImageNotFound);

            if (deleteImageFile) DeleteImageFile(tempImage, useTXFileManager);

            _unitOfWork.TempImagesRepository.Delete(tempImage);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task DeleteImagesAsync(IEnumerable<Image> images, bool deleteImageFile = true, bool useTXFileManager = true, CancellationToken cancellationToken = default)
        {
            if (deleteImageFile) DeleteImagesFiles(images, useTXFileManager);

            _unitOfWork.ImagesRepository.DeleteRange(images);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public void DeleteImageFile(Image image, bool useTXFileManager = true)
        {
            var fullPathToImage = _fileProvider.CombineWithRoot(image.VirtualPath, image.FileName);
            _fileProvider.DeleteFile(fullPathToImage, useTXFileManager);
        }

        public void DeleteImagesFiles(IEnumerable<Image> images, bool useTXFileManager = true)
        {
            var filesPaths = new List<string>();

            foreach (var image in images)
            {
                filesPaths.Add(_fileProvider.CombineWithRoot(image.VirtualPath, image.FileName));
            }

            _fileProvider.DeleteFiles(filesPaths, useTXFileManager);
        }

        public void DeleteImagesFiles(IEnumerable<TempImage> images, bool useTXFileManager = true)
        {
            var filesPaths = new List<string>();

            foreach (var image in images)
            {
                filesPaths.Add(_fileProvider.CombineWithRoot(image.VirtualPath, image.FileName));
                filesPaths.Add(_fileProvider.CombineWithRoot(image.VirtualPath, image.SmallVersionFileName));
            }

            _fileProvider.DeleteFiles(filesPaths, useTXFileManager);
        }

        public void DeleteImagesFiles(bool useTXFileManager = true, params Image[] images)
        {
            var filesPaths = new List<string>();

            foreach (var image in images)
            {
                filesPaths.Add(_fileProvider.CombineWithRoot(image.VirtualPath, image.FileName));
            }

            _fileProvider.DeleteFiles(filesPaths, useTXFileManager);
        }

        public async Task<Image> ResizeAndAddVersionOfImageAsync(string mainImageFilePath, string virtualPath, string extension, string postfix, int width, int height, bool useTXFileManager = true, CancellationToken cancellationToken = default)
        {
            (var imageFullPath, var imageFileName) = _fileProvider.FormNewFilePath(virtualPath, postfix, extension);

            var bytes = await _imageResizerService.ResizeAndGetBytesAsync(mainImageFilePath, width, height, cancellationToken);

            await _fileProvider.WriteAllBytesAsync(imageFullPath, bytes, useTXFileManager, cancellationToken);

            var addedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
            {
                FileName = imageFileName,
                MimeType = extension.ToMimeType(),
                VirtualPath = virtualPath
            }, cancellationToken);

            return addedImage;
        }

        public async Task<Image> ResizeAndAddVersionOfImageWithWatermarkAsync(string mainImageFilePath, string virtualPath, string extension, string postfix, int width, int height, byte[] watermarkImageBytes, float opacity, bool useTXFileManager = true, CancellationToken cancellationToken = default)
        {
            (var imageFullPath, var imageFileName) = _fileProvider.FormNewFilePath(virtualPath, postfix, extension);

            var bytes = await _imageResizerService.ResizeAndAddWatermarkAndGetBytesAsync(mainImageFilePath, width, height, watermarkImageBytes, opacity, cancellationToken);

            await _fileProvider.WriteAllBytesAsync(imageFullPath, bytes, useTXFileManager, cancellationToken);

            var addedImage = await _unitOfWork.ImagesRepository.AddAsync(new Image
            {
                FileName = imageFileName,
                MimeType = extension.ToMimeType(),
                VirtualPath = virtualPath,
            }, cancellationToken);

            return addedImage;
        }

        #region Helpers

        private void DeleteImageFile(TempImage image, bool useTXFileManager = true)
        {
            var fullPathToImage = _fileProvider.CombineWithRoot(image.VirtualPath, image.FileName);
            var fullPathToSmallVersionImage = _fileProvider.CombineWithRoot(image.VirtualPath, image.SmallVersionFileName);
            
            _fileProvider.DeleteFile(fullPathToImage, useTXFileManager);
            _fileProvider.DeleteFile(fullPathToSmallVersionImage, useTXFileManager);
        }

        #endregion
    }
}
