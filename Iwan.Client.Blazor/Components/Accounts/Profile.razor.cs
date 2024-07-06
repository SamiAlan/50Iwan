using AKSoftware.Blazor.Utilities;
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
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Accounts
{
    public partial class Profile
    {
        protected UpdateProfileDto _profile = new();
        protected bool _busy;
        protected UpdateProfileDtoValidator _profileValidator;
        protected ServerValidation _serverValidator;

        [Inject] public IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] public IAccountService AccountService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _profileValidator = new UpdateProfileDtoValidator(Localizer);

            _busy = true;

            _profile = (await AccountService.GetCurrentUserAsync()).MapToUpdateProfileDto();

            _busy = false;

            await base.OnInitializedAsync();
        }

        protected async Task UpdateProfileAsync()
        {
            _busy = true;

            try
            {
                var user = await AccountService.UpdateProfileAsync(_profile);
                MessagingCenter.Send(this, Messages.ProfileUpdated, user);
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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(_profile));

                return;
            }
            finally { _busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.ProfileUpdated), Severity.Success);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}