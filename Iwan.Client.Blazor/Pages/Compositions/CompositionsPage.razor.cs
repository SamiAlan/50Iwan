using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Compositions;
using Iwan.Shared;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Compositions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Compositions
{
    public partial class CompositionsPage
    {
        protected MudTable<CompositionDto> table;
        protected GetCompositionsOptions options = new() { IncludeImages = true };
        protected bool busy;

        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected ICompositionService CompositionService { get; set; }
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

        protected void EditComposition(string compositionId)
        {
            NavigationManager.NavigateTo(AppPages.EditComposition.ReplaceRouteParameters(compositionId));
        }

        protected void DeleteComposition(string compositionId)
        {
            DialogService.ShowConfirmationDialog(
                this, Localize(LocalizeKeys.DeleteComposition), Localize(LocalizeKeys.DeleteCompositionConfirmation),
                async () => await HandleDeleteCompositionConfirmedAsync(compositionId));
        }

        protected void ChangeCompositionImage(CompositionDto composition)
        {
            DialogService.ShowChangeCompositionImageDialog(this, composition.Id, HandleCompositionImageChangedAsync);
        }

        protected async Task HandleDeleteCompositionConfirmedAsync(string compositionId)
        {
            busy = true;

            try
            {
                await CompositionService.DeleteCompositionAsync(compositionId);
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

            Snackbar.Add(Localize(LocalizeKeys.DeleteCompositionSuccess), Severity.Success);

            await ReloadDataAsync();
        }

        protected async Task HandleCompositionImageChangedAsync()
        {
            Snackbar.Add(Localize(LocalizeKeys.CompositionImageChanged), Severity.Success);
            await ReloadDataAsync();
        }

        protected async Task<TableData<CompositionDto>> SearchData(TableState state)
        {
            busy = true;

            try
            {
                options.CurrentPage = state.Page;
                options.PageSize = state.PageSize;
                await SetOptions();

                var data = await CompositionService.GetCompositionsAsync(options);

                return new TableData<CompositionDto>
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

            return new TableData<CompositionDto>
            {
                TotalItems = 0,
                Items = new List<CompositionDto>()
            };
        }

        protected async Task ReloadDataAsync() => await table.ReloadServerData();

        #region Helpers

        private string Localize(string key, params object[] values) => Localizer.Localize(key, values);

        private async Task<GetCompositionsOptions> GetOrUpdateOptions()
        {
            if (!await LocalStorageService.ContainKeyAsync(nameof(GetCompositionsOptions)))
                await LocalStorageService.SetItemAsync(nameof(GetCompositionsOptions), options);

            return await LocalStorageService.GetItemAsync<GetCompositionsOptions>(nameof(GetCompositionsOptions));

        }

        private async Task SetOptions()
        {
            await LocalStorageService.SetItemAsStringAsync(nameof(GetCompositionsOptions), options.ToJson());
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
