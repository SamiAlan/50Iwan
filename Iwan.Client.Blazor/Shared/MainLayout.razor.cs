using Iwan.Client.Blazor.Constants;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Extensions;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        protected bool _rightToLeft = false;
        protected bool _drawerOpen = true;
        private readonly AppTheme _theme = new();

        [Inject]
        protected ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IStringLocalizer<Localization> Localizer { get; set; }

        private void RightToLeftToggle()
        {
            _rightToLeft = !_rightToLeft;
        }

        protected void NavigateToAccount()
        {
            NavigationManager.NavigateTo(AppPages.Account);
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected async Task ChangeLanguage(string languageCulture)
        {
            if (!AppLanguages.All().Contains(languageCulture))
                languageCulture = AppLanguages.English.Culture;

            await LocalStorageService.SetItemAsStringAsync(Keys.CultureCode, languageCulture);
            NavigationManager.NavigateTo(AppPages.Index, forceLoad: true, replace: true);
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

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}
