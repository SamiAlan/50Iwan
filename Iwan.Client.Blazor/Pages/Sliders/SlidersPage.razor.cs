using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.SliderImages;
using Iwan.Shared;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.SliderImages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Sliders
{
    public partial class SlidersPage
    {
        protected MudTable<SliderImageDto> table;
        protected GetSliderImagesOptions _options = new();
        protected bool busy;

        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] ISliderImageService SliderImageService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IDialogService DialogService { get; set; }
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

        protected void AddSliderImage()
        {
            DialogService.ShowAddSliderImageDialog(this, HandleSliderImageAdded);
        }

        protected async Task HandleSliderImageAdded()
        {
            Snackbar.Add(Localize(LocalizeKeys.SliderImageAdded), Severity.Success);
            await ReloadDataAsync();
        }

        protected async Task HandleSliderImageEdited()
        {
            Snackbar.Add(Localize(LocalizeKeys.SliderImageEdited), Severity.Success);
            await ReloadDataAsync();
        }

        protected async Task EditSliderImageAsync(string id)
        {
            busy = true;

            try
            {
                var sliderImage = await SliderImageService.GetSliderImageAsync(id);
                DialogService.ShowEditSliderImageDialog(this, sliderImage.MapToEditSliderImageDto(), HandleSliderImageEdited);
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
            }
            finally { busy = false; }
        }

        protected void DeleteSliderImage(string id)
        {
            DialogService.ShowConfirmationDialog(
                this, Localize(LocalizeKeys.DeleteSliderImage), Localize(LocalizeKeys.DeleteSliderImageConfirmation),
                async () => await HandleDeleteSliderImageConfirmedAsync(id));
        }

        protected async Task HandleDeleteSliderImageConfirmedAsync(string id)
        {
            busy = true;

            try
            {
                await SliderImageService.DeleteSliderImageAsync(id);
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

            Snackbar.Add(Localize(LocalizeKeys.DeleteSliderImageSuccess), Severity.Success);

            await ReloadDataAsync();
        }

        protected async Task<TableData<SliderImageDto>> SearchData(TableState state)
        {
            busy = true;

            try
            {
                _options.CurrentPage = state.Page;
                _options.PageSize = state.PageSize;

                var data = await SliderImageService.GetSliderImagesAsync(_options);

                return new TableData<SliderImageDto>
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

            return new TableData<SliderImageDto>
            {
                TotalItems = 0,
                Items = new List<SliderImageDto>()
            };
        }

        protected async Task ReloadDataAsync() => await table.ReloadServerData();

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);

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
