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
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Settings
{
    public partial class EditAboutUsSectionImagesSettings
    {
        private AboutUsSectionImagesSettingsDtoValidator _validator;

        protected AboutUsSectionImagesSettingsDto _settings = new();

        protected ServerValidation _serverValidator;

        protected bool _busy = false;

        [Parameter] public ImagesSettingsDto ImagesSettings { get; set; }
        [Inject] public IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] public ISettingsService SettingsService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IBackgroundJobsService BackgroundJobsService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _validator = new(Localizer);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (ImagesSettings != null)
                _settings = ImagesSettings.MapToAboutUsSectionImagesSettingsDto();
        }

        protected async Task UpdateSettingsAsync()
        {
            _busy = true;

            try
            {
                _settings = await SettingsService.UpdateSettingsAsync(_settings);
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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(_settings));

                return;
            }
            finally { _busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.AboutUsSectionImagesSettingsUpdated), Severity.Success);
        }

        protected async Task StartResizingJobAsync()
        {
            _busy = true;

            try
            {
                await BackgroundJobsService.StartResizingAboutUsImagesAsync();
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            finally
            {
                _busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.ResizingProductsImagesJobStarted), Severity.Success);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}