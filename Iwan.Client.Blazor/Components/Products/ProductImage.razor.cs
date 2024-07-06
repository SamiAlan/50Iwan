using Iwan.Client.Blazor.Services.Products;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Shared;
using Microsoft.Extensions.Localization;
using Iwan.Shared.Extensions;
using MudBlazor;

namespace Iwan.Client.Blazor.Components.Products
{
    public partial class ProductImage
    {
        protected bool hovering;
        protected bool deletingImage;

        [Parameter] public EventCallback<ProductImageDto> ImageDeleted { get; set; }
        [Parameter] public EventCallback<string> ErrorOccured { get; set; }
        [Parameter] public ProductImageDto Image { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IProductService ProductService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected async Task DeleteImageAsync()
        {
            if (deletingImage) return;

            deletingImage = true;

            try
            {
                await ProductService.DeleteProductImageAsync(Image.Id);

                if (ImageDeleted.HasDelegate)
                    await ImageDeleted.InvokeAsync(Image);
            }
            catch (UnAuthorizedException e)
            {
                if (ErrorOccured.HasDelegate)
                    await ErrorOccured.InvokeAsync(Localize(e.Message));
                else
                    Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            catch (ServiceException e)
            {
                if (ErrorOccured.HasDelegate)
                    await ErrorOccured.InvokeAsync(Localize(e.Message));
                else
                    Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            finally
            {
                deletingImage = false;
                hovering = false;
            }
        }

        private string Localize(string key, params string[] values) => Localizer.Localize(key, values);

    }
}