using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Compositions;
using Iwan.Client.Blazor.Services.Media;
using Iwan.Shared;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Compositions.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Compositions
{
    public partial class AddCompositionPage
    {
        #region Validators

        protected AddCompositionDtoValidator _compositionValidator;

        #endregion

        protected AddCompositionDto composition = new();
        protected bool busy = false;
        protected ServerValidation _serverValidator;

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected ICompositionService CompositionService { get; set; }
        [Inject] protected IImageService ImageService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            await base.OnInitializedAsync();

            _compositionValidator = new AddCompositionDtoValidator(Localizer);
        }

        protected async Task AddCompositionAsync()
        {
            busy = true;

            try
            {
                await CompositionService.AddCompositionAsync(composition);
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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(composition));

                return;
            }
            finally { busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.CompositionAdded), Severity.Success);
            Reset();
        }

        public void OnUploadingTempImage()
        {
            busy = true;
        }

        public void OnTempImageDeleted()
        {
            composition.Image = null;
        }

        public void OnTempImageUploaded(TempImageDto tempImageDto)
        {
            busy = false;
            composition.Image = new AddCompositionImageDto(new AddImageDto(tempImageDto.Id));
        }

        protected void GoToCompositionsPage() => NavigationManager.NavigateTo(AppPages.Compositions);

        protected void Reset()
        {
            composition = new();
            busy = false;
        }

        protected string Localize(string key, params object[] values) => Localizer.Localize(key, values);

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