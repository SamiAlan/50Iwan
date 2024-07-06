using Iwan.Server.BackgroundServices;
using Iwan.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Iwan.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application general services
    /// </summary>
    public class GeneralServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            // Add signalR
            services.AddSignalR();

            // Add the http context accessor
            services.AddHttpContextAccessor();

            // Add mediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Add localization
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = AppLanguages.All().Select(c => new CultureInfo(c)).ToList();

                options.ApplyCurrentCultureToResponseHeaders = true;
                options.DefaultRequestCulture = new RequestCulture(AppLanguages.English.Culture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddHttpClient("KeepAliveClient", client =>
            {
                client.BaseAddress = new Uri(configuration["BaseUrl"]);
            });
            // Return the services collection
            return services;
        }
    }
}
