using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Catalog;
using Iwan.Shared;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Catalog.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Catalog
{
    public partial class ChangeCategoryImageDialog
    {
        protected bool busy = false;
        protected ChangeCategoryImageDto categoryImage = new ChangeCategoryImageDto();
        protected ChangeCategoryImageDtoValidator _imageValidator;
        protected ServerValidation _serverValidator;

        [CascadingParameter] MudDialogInstance DialogInstance { get; set; }

        [Parameter] public EventCallback OnImageChanged { get; set; }

        [Parameter] public string CategoryId { get; set; }

        [Inject] public IStringLocalizer<Localization> Localizer { get; set; }

        [Inject] public ICategoryService CategoryService { get; set; }

        [Inject] public ISnackbar Snackbar { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _imageValidator = new(Localizer);
        }

        protected async Task ChangeImageAsync()
        {
            categoryImage.CategoryId = CategoryId;
            busy = true;
            try
            {
                await CategoryService.ChangeCategoryImageAsync(categoryImage);
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
                    Snackbar.Add(e.Message, Severity.Error);
                else
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(categoryImage));
                return;
            }
            finally
            {
                busy = false;
            }
        }

        protected void OnTempImageDeleted()
        {
            categoryImage.Image = null;
        }

        protected void OnTempImageUploaded(TempImageDto tempImage)
        {
            categoryImage.Image = new EditImageDto(tempImage.Id);
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