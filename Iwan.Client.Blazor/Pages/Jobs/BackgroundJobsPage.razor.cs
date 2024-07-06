using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Shared;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Jobs
{
    public partial class BackgroundJobsPage
    {
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            await base.OnInitializedAsync();
        }
        protected string Localize(string keys, params string[] values) => Localizer.Localize(keys, values);

        protected async Task Logout()
        {
            if (await LocalStorageService.ContainKeyAsync(Keys.AccessToken))
                await LocalStorageService.RemoveItemAsync(Keys.AccessToken);

            if (await LocalStorageService.ContainKeyAsync(Keys.RefreshToken))
                await LocalStorageService.RemoveItemAsync(Keys.RefreshToken);

            await AuthStateProvider.GetAuthenticationStateAsync();

            NavigationManager.NavigateTo(AppPages.Login);
        }
    }
}