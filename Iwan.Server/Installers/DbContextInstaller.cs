using Iwan.Server.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwan.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application db context
    /// </summary>
    public class DbContextInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add the application db context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Make it use sql server with the connnection string taken from the configurations
                options.UseSqlServer(configuration["Database:ConnectionStrings:SqlServerConnection"])
                    // Make it use lazy loading proxies for easier querying
                    .UseLazyLoadingProxies();
            });

            // Add the contract and the concrete versions of the db context
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();

            // Return the services collection
            return services;
        }
    }
}
