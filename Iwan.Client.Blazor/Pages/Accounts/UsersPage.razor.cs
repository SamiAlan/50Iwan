using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Accounts;
using Iwan.Shared;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Accounts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Accounts
{
    public partial class UsersPage
    {
        protected MudTable<UserDto> table;
        protected GetUsersOptions options = new();
        protected ClaimsPrincipal user;
        protected bool busy;

        [CascadingParameter] public Task<AuthenticationState> AuthState { get; set; }

        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }

        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Inject] protected IAccountService AccountService { get; set; }

        [Inject] protected ISnackbar Snackbar { get; set; }

        [Inject] protected IDialogService DialogService { get; set; }

        [Inject] protected ILocalStorageService LocalStorageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            if (AuthState != null)
                user = (await AuthState).User;
            
            options = await GetOrUpdateOptions();

            await base.OnInitializedAsync();
        }

        protected async Task ReloadDataAsync() => await table.ReloadServerData();

        protected void DeleteUserAsync(string id)
        {
            DialogService.ShowConfirmationDialog(
                this, Localize(LocalizeKeys.DeleteUser), Localize(LocalizeKeys.DeleteUserConfirmation),
                async () => await HandleDeleteUserAsync(id));
        }

        protected async Task HandleDeleteUserAsync(string id)
        {
            busy = true;

            try
            {
                await AccountService.DeleteUserAsync(id);
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.DeleteUserSuccess), Severity.Success);

            await ReloadDataAsync();
        }

        protected async Task<TableData<UserDto>> SearchData(TableState state)
        {
            busy = true;

            try
            {
                options.CurrentPage = state.Page;
                options.PageSize = state.PageSize;
                await SetOptions();

                var data = await AccountService.GetUsersAsync(options);

                return new TableData<UserDto>
                {
                    TotalItems = data.TotalCount,
                    Items = data.Data
                };
            }
            catch (UnAuthorizedException) { await Logout(); }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }

            finally
            {
                busy = false;
            }

            return new TableData<UserDto>
            {
                TotalItems = 0,
                Items = new List<UserDto>()
            };
        }

        #region Helpers

        private string Localize(string key, params object[] values) => Localizer.Localize(key, values);

        private async Task<GetUsersOptions> GetOrUpdateOptions()
        {
            if (!await LocalStorageService.ContainKeyAsync(nameof(GetUsersOptions)))
                await LocalStorageService.SetItemAsync(nameof(GetUsersOptions), options);

            return await LocalStorageService.GetItemAsync<GetUsersOptions>(nameof(GetUsersOptions));

        }

        private async Task SetOptions()
        {
            await LocalStorageService.SetItemAsStringAsync(nameof(GetUsersOptions), options.ToJson());
        }

        #endregion

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
