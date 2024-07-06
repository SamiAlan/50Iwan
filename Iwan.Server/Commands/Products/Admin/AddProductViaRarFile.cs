using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Iwan.Server.Services.Products;
using Iwan.Server.Services.Media;
using Microsoft.AspNetCore.Http;
using Iwan.Shared.Dtos.Products;
using System.Transactions;
using Iwan.Shared.Exceptions;
using Iwan.Server.Constants;
using Iwan.Server.Services.Common;
using Aspose.Zip.Rar;
using Iwan.Server.Infrastructure.Files;
using System.Collections.Generic;
using Iwan.Shared.Extensions;

namespace Iwan.Server.Commands.Products.Admin
{
    public class AddProductViaRarFile
    {
        public record Request(IFormFile RarFile) : IRequest<ProductDto>;

        public class Handler : IRequestHandler<Request, ProductDto>
        {
            protected readonly IMediator _mediator;
            protected readonly IProductService _productService;
            protected readonly IQueryProductService _queryProductService;
            protected readonly IRarFileService _rarFileService;
            protected readonly IExcelFileService _excelFileService;
            protected readonly IImageService _imageService;
            protected readonly IFileProvider _fileProvider;

            public Handler(IMediator mediator, IProductService productService,
                IQueryProductService queryProductService, IRarFileService rarFileService,
                IExcelFileService excelFileService, IImageService imageService, IFileProvider fileProvider)
            {
                _mediator = mediator;
                _productService = productService;
                _queryProductService = queryProductService;
                _rarFileService = rarFileService;
                _excelFileService = excelFileService;
                _imageService = imageService;
                _fileProvider = fileProvider;
            }

            public async Task<ProductDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        // Get rar file and get main-image, details, and images from it
                        using (var archive = new RarArchive(request.RarFile.OpenReadStream()))
                        {
                            var productAsRar = _rarFileService.GetProductAsRar(archive);

                            // Get relevant data from details file
                            var productToAdd = _excelFileService.GetProductDetailsFromExcelFile(productAsRar.Details.Open());

                            // Make initially visible
                            productToAdd.IsVisible = true;

                            // Add main image and other image as temp images
                            var mainImageFileName = _fileProvider.GetFileName(productAsRar.MainImage.Name);

                            try
                            {
                                var tempMainImage = await _imageService.UploadTempImageAsync
                                (productAsRar.MainImage.Open(), mainImageFileName, (long)productAsRar.MainImage.UncompressedSize,
                                 _fileProvider.GetFileExtension(mainImageFileName).ToMimeType(), true, cancellationToken);

                                productToAdd.MainImage = new(new(tempMainImage.Id));
                            } catch { }
                            

                            var productTempImages = new List<AddProductImageDto>();

                            foreach (var image in productAsRar.Images)
                            {
                                var tempImageFileName = _fileProvider.GetFileName(image.Name);
                                var productTempImage = await _imageService.UploadTempImageAsync
                                    (image.Open(), tempImageFileName, (long)image.UncompressedSize,
                                 _fileProvider.GetFileExtension(tempImageFileName).ToMimeType(), true, cancellationToken);

                                productTempImages.Add(new(new(productTempImage.Id)));
                            }

                            productToAdd.Images = productTempImages;

                            // Add the details along with the temp images
                            var product = await _productService.AddProductAsync(productToAdd, cancellationToken);

                            // Return the product details
                            var productDto = await _queryProductService.GetProductDetailsAsync(product.Id, true, true, cancellationToken);

                            scope.Complete();

                            return productDto;
                        }
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Products.ErrorAddingProduct); }
                }
            }
        }
    }
}
