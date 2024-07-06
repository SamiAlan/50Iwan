using Iwan.Server.DataAccess;
using Iwan.Server.Services.Media;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.BackgroundServices
{
    public class TempImagesBackgroundService : IHostedService, IDisposable
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        private const int _delayTime = 5 * 60 * 1000;
        private Timer _timer;

        public TempImagesBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected void DoBackgroundWork(object state = null)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var now = DateTime.UtcNow;

                var tempImagesToDelete = unitOfWork.TempImagesRepository.Where(i => i.ExpirationDate <= now).ToList();

                if (tempImagesToDelete.Any())
                {
                    var imageService = scope.ServiceProvider.GetService<IImageService>();
                    imageService.DeleteImagesFiles(tempImagesToDelete, false);
                    unitOfWork.TempImagesRepository.DeleteRange(tempImagesToDelete);
                    unitOfWork.SaveChanges(null);
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoBackgroundWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_delayTime));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
