using Iwan.Server.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwan.Server.Installers
{
    public class AppBackgroundServicesInstaller : IInstaller
    {
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddHostedService<TempImagesBackgroundService>();

            return services;
        }
    }
}
