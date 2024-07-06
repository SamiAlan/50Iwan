using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwan.Server.Installers
{
    public interface IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment);
    }
}
