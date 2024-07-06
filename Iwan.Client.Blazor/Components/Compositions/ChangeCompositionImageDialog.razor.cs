using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Compositions;
using Iwan.Shared;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Compositions.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Compositions
{
    public partial class ChangeCompositionImageDialog
    {
        protected bool busy = false;
        protected ChangeCompositionImageDto compositionImage = new ChangeCompositionImageDto();
        protected ChangeCompositionImageDtoValidator _imageValidator;
        protected ServerValidation _serverValidator;
        [CascadingParameter]
        MudDialogInstance DialogInstance { get; set; }

        [Parameter]
        public EventCallback OnImageChanged { get; set; }

        [Parameter]
        public string CompositionId { get; set; }

        [Inject]
        public IStringLocalizer<Localization> Localizer { get; set; }

        [Inject]
        public ICompositionService CompositionService { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _imageValidator = new(Localizer);
        }

        protected async Task ChangeImageAsync()
        {
            compositionImage.CompositionId = CompositionId;
            busy = true;
            try
            {
                await CompositionService.ChangeCompositionImageAsync(compositionImage);
                if (OnImageChanged.HasDelegate)
                    await OnImageChanged.InvokeAsync();
                DialogInstance.Close(DialogResult.Ok(true));
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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(compositionImage));
                return;
            }
            finally
            {
                busy = false;
            }
        }

        protected void OnTempImageDeleted()
        {
            compositionImage.Image = null;
        }

        protected void OnTempImageUploaded(TempImageDto tempImage)
        {
            compositionImage.Image = new EditImageDto(tempImage.Id);
            busy = false;
        }

        protected void OnUploadingTempImage()
        {
            busy = true;
        }

        protected void HandleCancelClicked()
        {
            DialogInstance.Close(DialogResult.Ok(false));
        }

        protected string Localize(string key, params object[] values) => Localizer.Localize(key, values);
    }
}