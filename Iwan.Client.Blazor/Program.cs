using Blazored.LocalStorage;
using Iwan.Client.Blazor;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.MessageHandlers;
using Iwan.Client.Blazor.Infrastructure.Security.Providers;
using Iwan.Shared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient(HttpClientsNames.Base, client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<AuthorizationMessageHandler>()
    .AddHttpMessageHandler<CultureMessageHandler>();

builder.Services.AddHttpClient(HttpClientsNames.Auth, client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddTransient<AuthorizationMessageHandler>();
builder.Services.AddTransient<CultureMessageHandler>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddApplicationServices();
builder.Services.AddValidators();
builder.Services.AddAuthorizationCore();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
var host = builder.Build();

var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
var cultureCode = await localStorage.GetItemAsStringAsync(Keys.CultureCode);

if (string.IsNullOrEmpty(cultureCode))
{
    cultureCode = AppLanguages.English.Culture;
    await localStorage.SetItemAsStringAsync(Keys.CultureCode, cultureCode);
}

var culture = new CultureInfo(cultureCode);

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
