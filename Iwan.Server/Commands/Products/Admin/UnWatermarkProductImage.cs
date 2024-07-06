using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Products;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{

    public class UnWatermarkProductImage
    {
        public record Request(string Id) : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IUnitOfWork _context;
            private readonly IImageManipulatorService _imageManipulatorService;
            private readonly IQuerySettingService _querySettingService;
            private readonly IQueryProductService _queryProductService;
            private readonly IFileProvider _fileProvider;

            public Handler(IUnitOfWork context, IQuerySettingService querySettingService,
                IImageManipulatorService imageManipulatorService, IQueryProductService queryProductService,
                IFileProvider fileProvider)
            {
                _context = context;
                _querySettingService = querySettingService;
                _imageManipulatorService = imageManipulatorService;
                _queryProductService = queryProductService;
                _fileProvider = fileProvider;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var imagesSettings = await _querySettingService.GetProductsImagesSettingsAsync(cancellationToken);

                var productMainImage = await _context.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(request.Id, cancellationToken);

                if (productMainImage is null)
                    throw new NotFoundException(Responses.Products.MainImageNotFound);

                var originalMainImageBytes = await _imageManipulatorService.GetBytesAsync(productMainImage.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, originalMainImageBytes, cancellationToken);
                
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.SmallImage, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, originalMainImageBytes, cancellationToken);

                var fullOldPath = _fileProvider.CombineWithRoot(productMainImage.MobileImage.VirtualPath, productMainImage.MobileImage.FileName);
                _fileProvider.DeleteFile(fullOldPath);

                var extension = Path.GetExtension(productMainImage.MobileImage.FileName);

                productMainImage.MobileImage.FileName = $"{Guid.NewGuid()}_product_mobile{extension}";

                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, originalMainImageBytes, cancellationToken);

                var productImages = await _context.ProductImagesRepository.GetImagesWithImageModelsAsync(request.Id, cancellationToken);

                // Resize sub-images
                foreach (var subImage in productImages)
                {
                    var originalSubImageBytes = await _imageManipulatorService.GetBytesAsync(subImage.OriginalImage, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, originalSubImageBytes, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.SmallImage, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, originalSubImageBytes, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, originalSubImageBytes, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
