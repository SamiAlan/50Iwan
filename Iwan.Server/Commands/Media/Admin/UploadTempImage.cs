using Iwan.Shared.Exceptions;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Media;
using Iwan.Server.Constants;
using Iwan.Shared.Dtos.Media;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Iwan.Server.Infrastructure.Files;

namespace Iwan.Server.Commands.Media.Admin
{
    public class UploadTempImage
    {
        public record Request(IFormFile ImageFile) : IRequest<TempImageDto>;

        public class Handler : IRequestHandler<Request, TempImageDto>
        {
            protected readonly IImageService _imageService;
            protected readonly IAppImageHelper _appImageHelper;
            protected readonly IImageManipulatorService _imageResizerService;
            protected readonly IFileProvider _fileProvider;

            public Handler(IImageService tempImageService, IAppImageHelper appImageHelper,
                IImageManipulatorService imageResizerService, IFileProvider fileProvider)
            {
                _imageService = tempImageService;
                _appImageHelper = appImageHelper;
                _imageResizerService = imageResizerService;
                _fileProvider = fileProvider;
            }

            public async Task<TempImageDto> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var tempImage = await _imageService.UploadTempImageAsync(request.ImageFile, true, cancellationToken);

                        scope.Complete();

                        return _appImageHelper.GenerateImageDto(tempImage);
                    }
                    catch (BaseException) { throw; }
                    catch { throw new ServerErrorException(Responses.Images.ErrorWhileUploading); }
                }
            }
        }
    }
}
