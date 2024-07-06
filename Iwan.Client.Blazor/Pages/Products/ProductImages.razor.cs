using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Shared;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Products
{
    public partial class ProductImages
    {
        private List<ProductImageDto> productImages = new();
        private bool busy;

        [Parameter] public string Id { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IProductService ProductService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            if (!Id.IsNullOrEmptyOrWhiteSpaceSafe())
                productImages = (await ProductService.GetProductImagesAsync(Id)).ToList();
            await base.OnInitializedAsync();
        }

        protected void OnImageDeleted(ProductImageDto image)
        {
            productImages.Remove(image);
        }

        protected async Task OnTempImageUploaded(TempImageDto tempImage)
        {
            busy = true;

            try
            {
                var productImage = await ProductService.AddProductImageAsync(new AddProductImageDto
                {
                    ProductId = Id,
                    Image = new AddImageDto(tempImage.Id)
                });

                productImages.Add(productImage);
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
            }
            finally
            {
                busy = false;
            }
        }

        protected void GoToProductsPage()
        {
            NavigationManager.NavigateTo(AppPages.Products);
        }

        private string Localize(string key, params string[] values) => Localizer.Localize(key, values);

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