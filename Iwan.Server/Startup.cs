using Hangfire;
using Iwan.Server.Constants;
using Iwan.Server.Extensions;
using Iwan.Server.Hubs;
using Iwan.Server.Infrastructure.Files;
using Iwan.Server.Infrastructure.Middlewares;
using Iwan.Server.Options;
using Iwan.Shared.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Iwan.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });

            // Run all dependencies installers
            services.RunDependenciesInstallers(Configuration, WebHostEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            // Use global exception handling middleware
            app.UseMiddleware<ExceptionsHandlingMiddleware>();

            // Use http redirection
            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();

            app.UseStaticFiles();

            // Use routing
            app.UseRouting();

            // Use cors
            app.UseCors("DefaultCorsPolicy");

            // Use Authentication
            app.UseAuthentication();

            // Use Authorization
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                name: "LocalizedDefault",
                pattern: "{lang:lang}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "",
                    defaults: new { controller = "Home", action = "RedirectToDefaultLanguage", lang = "en-US" });

                endpoints.MapFallbackToFile("index.html");

                // endpoints.MapHangfireDashboardWithAuthorizationPolicy(Policies.HangfireDashboard.Name);

                // Map the SignalR admin hub
                //endpoints.MapHub<AdminHub>("/admin-hub", (options) =>
                //{
                //    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(3600);
                //});
            });

            MakeSureNecessaryDirectoriesExist(serviceProvider);
        }

        private static void MakeSureNecessaryDirectoriesExist(IServiceProvider serviceProvider)
        {
            var webRootPath = serviceProvider.GetRequiredService<PathsOptions>().WebRootPath;
            var fileProvider = serviceProvider.GetRequiredService<IFileProvider>();

            var imagesDirectoryPath = fileProvider.Combine(webRootPath, AppDirectories.Images.SELF);
            fileProvider.CreateDirectory(imagesDirectoryPath);

            var categoriesDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Categories);
            fileProvider.CreateDirectory(categoriesDirectoryName);

            var compositionsDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Compositions);
            fileProvider.CreateDirectory(compositionsDirectoryName);

            var productsDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Products);
            fileProvider.CreateDirectory(productsDirectoryName);

            var tempDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Temp);
            fileProvider.CreateDirectory(tempDirectoryName);

            var sliderDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Slider);
            fileProvider.CreateDirectory(sliderDirectoryName);

            var aboutUsDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.AboutUs);
            fileProvider.CreateDirectory(aboutUsDirectoryName);

            var interiorDesignDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.InteriorDesign);
            fileProvider.CreateDirectory(interiorDesignDirectoryName);

            var watermarkDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Watermark);
            fileProvider.CreateDirectory(watermarkDirectoryName);

            var otherDirectoryName = fileProvider.Combine(imagesDirectoryPath, AppDirectories.Images.Other);
            fileProvider.CreateDirectory(otherDirectoryName);
        }
    }

    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo>
                                {
                                    new CultureInfo(AppLanguages.English.Culture),
                                    new CultureInfo(AppLanguages.Arabic.Culture)
                                };

            var options = new RequestLocalizationOptions()
            {

                DefaultRequestCulture = new RequestCulture(culture: AppLanguages.English.Culture, uiCulture: AppLanguages.English.Culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider() { Options = options, RouteDataStringKey = "lang", UIRouteDataStringKey = "lang" });

            app.UseRequestLocalization(options);
        }
    }

    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("lang"))
            {
                return false;
            }

            var lang = values["lang"].ToString();

            return AppLanguages.All().Contains(lang);
        }
    }
}
