using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Media;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Products;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Products.Admin
{
    public class WatermarkProductImage
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
                var watermarkSettings = await _querySettingService.GetWatermarkImageSettingsAsync(cancellationToken);

                if (watermarkSettings.WatermarkImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                    throw new BadRequestException(Responses.Settings.CantWatermarkWithoutImage);

                var watermarkImageBytes = await _imageManipulatorService.GetBytesAsync(watermarkSettings.WatermarkImage, cancellationToken);

                var productMainImage = await _context.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(request.Id, cancellationToken);

                if (productMainImage is null)
                    throw new NotFoundException(Responses.Products.MainImageNotFound);

                var productImages = await _context.ProductImagesRepository.GetImagesWithImageModelsAsync(request.Id, cancellationToken);

                var extension = Path.GetExtension(productMainImage.MobileImage.FileName);
                var newFileName = $"{Guid.NewGuid()}_product_mobile{extension}";

                var fullOldPath = _fileProvider.CombineWithRoot(productMainImage.MobileImage.VirtualPath, productMainImage.MobileImage.FileName);
                var fullNewPath = _fileProvider.CombineWithRoot(productMainImage.MobileImage.VirtualPath, newFileName);
                File.Move(fullOldPath, fullNewPath);

                productMainImage.MobileImage.FileName = newFileName;

                var images = new List<Image>
                { productMainImage.MediumImage, productMainImage.SmallImage, productMainImage.MobileImage };

                foreach (var subImage in productImages)
                    images.AddRange(new List<Image> { subImage.MediumImage, subImage.SmallImage, subImage.MobileImage });

                // Add watermark to the images
                await _imageManipulatorService.AddWatermarkToImagesAsync(images, watermarkImageBytes, watermarkSettings.Opacity, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
