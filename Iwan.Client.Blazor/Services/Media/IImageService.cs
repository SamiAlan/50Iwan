using Iwan.Shared.Dtos.Media;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System;

namespace Iwan.Client.Blazor.Services.Media
{
    public interface IImageService
    {
        Task<TempImageDto> UploadTempImageAsync(Stream fileSteam, string fileName, Action<long, double> onProgress = null, CancellationToken cancellationToken = default);
        Task DeleteTempImageAsync(string tempImageId, CancellationToken cancellationToken = default);
    }
}
