using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Jobs;
using Iwan.Client.Blazor.Services.Settings;
using Iwan.Shared;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Settings
{
    public partial class WatermarkSettingsPage
    {
        protected bool busy;
        protected EditWatermarkSettingsDto _editSettings = new();
        protected WatermarkImageDto _image;
        protected WatermarkSettingsDtoValidator _validator;
        protected ServerValidation _serverValidator;

        [Inject] public IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] public ISettingsService SettingsService { get; set; }
        [Inject] public IBackgroundJobsService BackgroundJobService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            _validator = new(Localizer);

            var _settings = await SettingsService.GetWatermarkSettingsAsync();
            
            _editSettings = _settings.ToEditDto();
            _image = _settings.WatermarkImage;

            await base.OnInitializedAsync();
        }

        protected async Task UpdateSettingsAsync()
        {
            busy = true;

            try
            {
                _editSettings = (await SettingsService.UpdateSettingsAsync(_editSettings)).ToEditDto();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                if (!e.Errors.Any())
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
                else
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(_editSettings));

                return;
            }
            finally { busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.WatermarkSettingsUpdated), Severity.Success);
        }

        protected async Task StartWatermarkingBackgroundJobAsync()
        {
            busy = true;

            try
            {
                await BackgroundJobService.StartWatermarkingJobAsync();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch(ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.WatermarkingJobStarted), Severity.Success);
        }

        protected async Task StartUnWatermarkingBackgroundJobAsync()
        {
            busy = true;

            try
            {
                await BackgroundJobService.StartUnWatermarkingJobAsync();
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

            Snackbar.Add(Localize(LocalizeKeys.UnWatermarkingJobStarted), Severity.Success);
        }

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