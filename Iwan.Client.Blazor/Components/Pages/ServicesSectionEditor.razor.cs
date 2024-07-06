using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Pages;
using Iwan.Shared;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Pages
{
    public partial class ServicesSectionEditor
    {
        protected EditServicesSectionDto _servicesSection = new();
        protected ServerValidation _serverValidator;
        protected bool busy;

        [Parameter] public ServicesSectionDto ServicesSection { get; set; }
        [Inject] public IPagesService PagesService { get; set; }
        [Inject] public IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        protected override void OnParametersSet()
        {
            if (ServicesSection != null)
                _servicesSection = ServicesSection.MapToEditServicesSectionDto();

            base.OnParametersSet();
        }

        protected async Task UpdateSectionAsync()
        {
            busy = true;

            try
            {
                _servicesSection = (await PagesService.EditSectionAsync(_servicesSection)).MapToEditServicesSectionDto();
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
                return;
            }
            catch (ServiceException e)
            {
                if (!e.Errors.Any())
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
                else
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(_servicesSection));
                return;
            }
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.ServicesSectionUpdated), Severity.Success);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}