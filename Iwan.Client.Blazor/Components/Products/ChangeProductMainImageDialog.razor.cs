using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Shared;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Products.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Products
{
    public partial class ChangeProductMainImageDialog
    {
        protected bool busy = false;
        protected ChangeProductMainImageDto productMainImage = new();
        protected ChangeProductMainImageDtoValidator _imageValidator;
        protected ServerValidation _serverValidator;

        [CascadingParameter] MudDialogInstance DialogInstance { get; set; }

        [Parameter] public EventCallback OnImageChanged { get; set; }

        [Parameter] public string ProductId { get; set; }

        [Inject] public IStringLocalizer<Localization> Localizer { get; set; }

        [Inject] public IProductService ProductService { get; set; }

        [Inject] public ISnackbar Snackbar { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _imageValidator = new(Localizer);
        }

        protected async Task ChangeImageAsync()
        {
            productMainImage.ProductId = ProductId;

            busy = true;
            
            try
            {
                await ProductService.ChangeProductMainImageAsync(productMainImage);

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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(productMainImage));
                return;
            }
            finally
            {
                busy = false;
            }
        }

        protected void OnTempImageDeleted()
        {
            productMainImage.Image = null;
        }

        protected void OnTempImageUploaded(TempImageDto tempImage)
        {
            productMainImage.Image = new EditImageDto(tempImage.Id);
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