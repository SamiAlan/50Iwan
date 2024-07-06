using Iwan.Client.Blazor.Services.Authentication;
using Iwan.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Iwan.Shared.Validators.Accounts.Admin;
using Iwan.Shared.Dtos.Accounts;
using System.Threading.Tasks;
using System;
using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Shared.Extensions;
using MudBlazor;

namespace Iwan.Client.Blazor.Pages.Authentication
{
    public partial class Login : ComponentBase
    {
        private readonly LoginDto loginCredentials = new();
        private LoginDtoValidator validator;
        private bool busy = false;
        bool PasswordVisibility;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        [Inject] protected IAuthService AuthService { get; set; }
        [Inject] protected ILocalStorageService LocalStorage { get; set; }
        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] protected NavigationManager Navigator { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            validator = new LoginDtoValidator(Localizer);
        }

        void TogglePasswordVisibility()
        {
            if(PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        protected async Task LoginAsync()
        {
            busy = true;
            try
            {
                var authToken = (await AuthService.LoginAsync(loginCredentials)).Data;

                if (await LocalStorage.ContainKeyAsync(Keys.AccessToken))
                    await LocalStorage.RemoveItemAsync(Keys.AccessToken);

                if (await LocalStorage.ContainKeyAsync(Keys.RefreshToken))
                    await LocalStorage.RemoveItemAsync(Keys.RefreshToken);

                await LocalStorage.SetItemAsStringAsync(Keys.AccessToken, authToken.Token);
                await LocalStorage.SetItemAsStringAsync(Keys.RefreshToken, authToken.RefreshToken);
                await AuthStateProvider.GetAuthenticationStateAsync();

                var snackbar = Snackbar.Add(Localize(LocalizeKeys.LoginSuccess), Severity.Success, options =>
                {
                    options.ShowTransitionDuration = 500;
                    options.VisibleStateDuration = 100;
                    options.HideTransitionDuration = 500;
                    options.Icon = Icons.Material.Filled.Security;
                });

                snackbar.OnClose += (s) => Navigator.NavigateTo(AppPages.Index);
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error, options =>
                {
                    options.ShowTransitionDuration = 1;
                    options.Icon = Icons.Material.Filled.Security;
                });
            }
            finally
            {
                busy = false;
            }
        }

        protected string Localize(string key, params object[] values) => Localizer.Localize(key, values);
    }
}
