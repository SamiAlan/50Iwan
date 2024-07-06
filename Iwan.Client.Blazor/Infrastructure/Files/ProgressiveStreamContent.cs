using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Threading;

namespace Iwan.Client.Blazor.Infrastructure.Files
{
    public class ProgressiveStreamContent : StreamContent
    {
        private readonly Stream _fileStream;
        private readonly int _maxBuffer = 1024 * 4;

        public ProgressiveStreamContent(Stream stream, int maxBuffer, Action<long, double> onProgress) : base(stream)
        {
            _fileStream = stream;
            _maxBuffer = maxBuffer;
            OnProgress += onProgress;
        }

        public event Action<long, double> OnProgress;


        protected async override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {

            var buffer = new byte[_maxBuffer];
            var totalLength = _fileStream.Length;

            long uploaded = 0;


            using (_fileStream)
            {
                while (true)
                {

                    var length = await _fileStream.ReadAsync(buffer, 0, _maxBuffer);

                    if (length <= 0)
                    {
                        break;
                    }

                    uploaded += length;
                    var perentage = Convert.ToDouble(uploaded * 100 / _fileStream.Length);

                    await stream.WriteAsync(buffer);

                    OnProgress?.Invoke(uploaded, perentage);
                }
            }
        }

    }
}
