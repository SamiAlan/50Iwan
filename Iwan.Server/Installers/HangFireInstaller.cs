using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using System;

namespace Iwan.Server.Installers
{
    public class HangFireInstaller : IInstaller
    {
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            //services.AddHangfire(globalConfig => globalConfig
            //    .UseSqlServerStorage(configuration["Database:ConnectionStrings:SqlServerConnection"]));

            //// Add the processing server as IHostedService
            //services.AddHangfireServer();

            return services;
        }
    }
}
