using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Accounts;
using Iwan.Shared;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Accounts.Admin;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Accounts
{
    public partial class ChangePassword
    {
        protected ChangePasswordDto _passwordRequest = new();
        protected bool _busy;
        protected ChangePasswordDtoValidator _changePasswordValidator;
        protected ServerValidation _serverValidator;

        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IAccountService AccountService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] NavigationManager Navigator { get; set; }
        [Inject] ILocalStorageService LocalStorageService { get; set; }
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _changePasswordValidator = new ChangePasswordDtoValidator(Localizer);

            _busy = true;

            _busy = false;

            await base.OnInitializedAsync();
        }

        protected async Task ChangePasswordAsync()
        {
            _busy = true;

            try
            {
                await AccountService.ChangePasswordAsync(_passwordRequest);
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                return;
            }
            catch (ServiceException e)
            {
                if (!e.Errors.Any())
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
                else
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(_passwordRequest));

                return;
            }
            finally { _busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.PasswordChanged), Severity.Success);

            await Task.Delay(2000);
            await Logout();
        }

        protected async Task Logout()
        {
            if (await LocalStorageService.ContainKeyAsync(Keys.AccessToken))
                await LocalStorageService.RemoveItemAsync(Keys.AccessToken);

            if (await LocalStorageService.ContainKeyAsync(Keys.RefreshToken))
                await LocalStorageService.RemoveItemAsync(Keys.RefreshToken);

            await AuthStateProvider.GetAuthenticationStateAsync();

            Navigator.NavigateTo(AppPages.Login);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}