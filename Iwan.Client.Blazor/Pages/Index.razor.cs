using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Services.Dashboard;
using Iwan.Shared;
using Iwan.Shared.Dtos.Dashboard;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages
{
    public partial class Index
    {
        protected DashboardDto data = new();
        [Inject] IDashboardService DashboardService { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                data = await DashboardService.GetDashboardDataAsync();
            }
            catch (UnAuthorizedException)
            {
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();

                if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                    await Logout();
            }

            await base.OnInitializedAsync();
        }

        protected async Task Logout()
        {
            if (await LocalStorageService.ContainKeyAsync(Keys.AccessToken))
                await LocalStorageService.RemoveItemAsync(Keys.AccessToken);

            if (await LocalStorageService.ContainKeyAsync(Keys.RefreshToken))
                await LocalStorageService.RemoveItemAsync(Keys.RefreshToken);

            await AuthStateProvider.GetAuthenticationStateAsync();

            NavigationManager.NavigateTo(AppPages.Login);
        }

        protected string Localize(string key, params object[] values)=> Localizer.Localize(key, values);
    }
}