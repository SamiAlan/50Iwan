using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Shared;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Products
{
    public partial class AddProductViaRar
    {
        bool busy;
        string _dragEnterStyle;
        double uploadProgress;
        IBrowserFile rarFile;
        protected ServerValidation _serverValidator;
        protected AddProductViaRarFileDto product = new();

        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IProductService ProductService { get; set; }

        void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            rarFile = e.GetMultipleFiles()[0];
        }

        protected async Task AddProductAsync()
        {
            busy = true;

            try
            {
                var product = await ProductService.AddProductViaRarFileAsync(rarFile.OpenReadStream(50000000), rarFile.Name, (_, p) =>
                {
                    uploadProgress = p;
                    StateHasChanged();
                });

                rarFile = null;
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
                busy = false;
                uploadProgress = 0;
            }

            Snackbar.Add(Localize(LocalizeKeys.ProductAdded), Severity.Success);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}