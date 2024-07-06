using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Services.Vendors;
using Iwan.Shared;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Vendors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Vendors
{
    public partial class VendorsPage : ComponentBase
    {
        protected MudTable<VendorDto> table;
        protected GetVendorsOptions options = new();
        protected bool busy;

        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IVendorService VendorService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            options = await GetOrUpdateOptions();

            await base.OnInitializedAsync();
        }

        protected void EditVendor(string vendorId)
        {
            busy = true;
            
            NavigationManager.NavigateTo(AppPages.EditVendor.ReplaceRouteParameters(vendorId));

            busy = false;
        }

        protected void DeleteVendorAsync(string vendorId)
        {
            DialogService.ShowConfirmationDialog(
                this, Localize(LocalizeKeys.DeleteVendor), Localize(LocalizeKeys.DeleteVendorConfirmation),
                async () => await HandleDeleteVendorConfirmed(vendorId));
        }

        protected async Task HandleDeleteVendorConfirmed(string vendorId)
        {
            busy = true;

            try
            {
                await VendorService.DeleteVendorAsync(vendorId);
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                return;
            }
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.DeleteVendorSuccess), Severity.Success);

            await ReloadDataAsync();
        }

        protected async Task<TableData<VendorDto>> SearchData(TableState state)
        {
            busy = true;

            try
            {
                options.CurrentPage = state.Page;
                options.PageSize = state.PageSize;
                await SetOptions();

                var data = await VendorService.GetVendorsAsync(options);

                return new TableData<VendorDto>
                {
                    TotalItems = data.TotalCount,
                    Items = data.Data
                };
            }
            catch (UnAuthorizedException ex) { await Logout(); }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
                
            finally
            {
                busy = false;
            }

            return new TableData<VendorDto>
            {
                TotalItems = 0,
                Items = new List<VendorDto>()
            };
        }

        protected async Task ReloadDataAsync() => await table.ReloadServerData();

        #region Helpers

        private string Localize(string key, params object[] values) => Localizer.Localize(key, values);

        private async Task<GetVendorsOptions> GetOrUpdateOptions()
        {
            if (!await LocalStorageService.ContainKeyAsync(nameof(GetVendorsOptions)))
                await LocalStorageService.SetItemAsync(nameof(GetVendorsOptions), options);

            return await LocalStorageService.GetItemAsync<GetVendorsOptions>(nameof(GetVendorsOptions));

        }

        private async Task SetOptions()
        {
            await LocalStorageService.SetItemAsStringAsync(nameof(GetVendorsOptions), options.ToJson());
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
