using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Validators.Common;
using Iwan.Shared.Validators.Vendors.Admin;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Iwan.Client.Blazor.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Get all classes that have the Injected attribute
            var injectedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(InjectedAttribute)).Any(a => !(a as InjectedAttribute).IgnoreForNow))
                    .Select(t => new
                    {
                        Type = t,
                        InjectedAttribute = t.GetCustomAttribute<InjectedAttribute>()
                    });

            // Loop over all class
            foreach (var injectedType in injectedTypes)
            {
                // Check the lifetiem and register the service accordingly
                switch (injectedType.InjectedAttribute.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(injectedType.InjectedAttribute.ContractType, injectedType.Type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(injectedType.InjectedAttribute.ContractType, injectedType.Type);
                        break;
                    default:
                        services.AddTransient(injectedType.InjectedAttribute.ContractType, injectedType.Type);
                        break;
                }
            }

            // Return the services collection
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddScoped<AddVendorDtoValidator>()
                .AddScoped<EditVendorDtoValidator>()
                .AddScoped<EditAddressDtoValidator>()
                .AddScoped<AddAddressDtoValidator>()
                .AddScoped<EditAddressDtoValidator>();
        }
    }
}
