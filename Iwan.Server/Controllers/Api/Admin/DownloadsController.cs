using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Options;
using Iwan.Shared;
using Iwan.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class DownloadsController : BaseController
    {
        private readonly IFileProvider _fileProvider;

        public DownloadsController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        [Route(Routes.Api.Admin.Downloads.ImageManager)]
        public async Task<IActionResult> DownloadImageManager(CancellationToken cancellationToken = default)
        {
            var path = _fileProvider.CombineWithRoot("IwanImageManager.rar");
            return File(await System.IO.File.ReadAllBytesAsync(path, cancellationToken), "application/vnd.rar", "IwanImageManager.rar");
        }
    }
}
