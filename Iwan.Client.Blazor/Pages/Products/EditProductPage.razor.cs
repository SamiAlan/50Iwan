using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Client.Blazor.Services.Vendors;
using Iwan.Shared;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Products.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Products
{
    public partial class EditProductPage
    {
        #region Validators

        protected EditProductDtoValidator _validator;

        #endregion

        protected EditProductDto product = new();
        protected bool busy = false;
        protected ServerValidation _serverValidator;
        protected IEnumerable<VendorDto> _vendors = new List<VendorDto>();

        [Parameter] public string Id { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IProductService ProductService { get; set; }
        [Inject] protected IVendorService VendorService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            _validator = new(Localizer);

            product = (await ProductService.GetProductByIdAsync(Id)).MapToEditProductDto();
            _vendors = await VendorService.GetAllVendorsAsync();

            await base.OnInitializedAsync();
        }

        protected async Task EditProductAsync()
        {
            busy = true;

            try
            {
                product = (await ProductService.EditProductAsync(product)).MapToEditProductDto();
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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(product));

                return;
            }
            finally { busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.ProductEdited), Severity.Success);
        }

        protected void GoToProductsPage() => NavigationManager.NavigateTo(AppPages.Products);

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