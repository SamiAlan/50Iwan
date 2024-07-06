using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Pages
{
    public partial class ProductDetailsPage
    {
        protected ProductDetailsPageContentDto _details = new();

        [Inject] IPagesService PagesService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            _details = await PagesService.GetProductDetailsPageContentAsync();
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
    }
}