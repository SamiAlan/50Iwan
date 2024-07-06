﻿using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwan.Server.Installers
{
    /// <summary>
    /// Represents a dependencies installer to the application authorization services
    /// </summary>
    public class AuthorizationServicesInstaller : IInstaller
    {
        /// <summary>
        /// Installs dependencies
        /// </summary>
        public IServiceCollection InstallDependencies(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            // Add authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.HangfireDashboard.Name, p =>
                {
                    p.RequireRole(Policies.HangfireDashboard.RequiredRoles);
                });
            }) // Add identity user type
               .AddIdentityCore<AppUser>(options =>
                {
                    // Password options
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = true;

                    // SignIn options
                    options.SignIn.RequireConfirmedAccount = false;

                    // User options
                    options.User.RequireUniqueEmail = true;
                })
                // Add roles type
                .AddRoles<IdentityRole>()
                // Add ef core stores
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // Add token providers
                .AddDefaultTokenProviders();

            // Return the services collection
            return services;
        }
    }
}
