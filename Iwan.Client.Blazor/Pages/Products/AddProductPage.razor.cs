using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Products;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Client.Blazor.Services.Vendors;
using Iwan.Shared;
using Iwan.Shared.Dtos.Media;
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
    public partial class AddProductPage
    {
        #region Validators

        protected AddProductDtoValidator _validator;

        #endregion

        protected AddProductDto product = new();
        protected bool busy = false;
        protected ServerValidation _serverValidator;
        protected IEnumerable<VendorDto> _vendors = new List<VendorDto>();
        protected List<TempImageDto> uploadedTempProductImages = new();
        protected ProductStatesEditor _statesEditor;

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

            _validator = new (Localizer);

            _vendors = await VendorService.GetAllVendorsAsync();

            await base.OnInitializedAsync();
        }

        protected async Task AddProductAsync()
        {
            busy = true;

            if (uploadedTempProductImages.Any())
                product.Images = uploadedTempProductImages.Select(i => new AddProductImageDto(new(i.Id))).ToList();
            
            if (_statesEditor.TempStates.Any())
                product.States = _statesEditor.TempStates;

            try
            {
                await ProductService.AddProductAsync(product);
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

            Snackbar.Add(Localize(LocalizeKeys.ProductAdded), Severity.Success);
            Reset();
        }

        public void OnUploadingTempMainImage()
        {
            busy = true;
        }

        public void OnUploadingProductTempImage()
        {
            busy = true;
        }

        public void OnTempMainImageDeleted(TempImageDto deletedImage)
        {
            product.MainImage = null;
        }

        public void OnProductTempImageDeleted(TempImageDto deletedImage)
        {
            uploadedTempProductImages.Remove(deletedImage);
        }

        public void OnTempMainImageUploaded(TempImageDto tempImageDto)
        {
            busy = false;
            product.MainImage = new(new AddImageDto(tempImageDto.Id));
        }

        public void OnProductTempImageUploaded(TempImageDto tempImageDto)
        {
            busy = false;
            uploadedTempProductImages.Add(tempImageDto);
        }

        protected void GoToProductsPage() => NavigationManager.NavigateTo(AppPages.Products);

        protected void Reset()
        {
            product = new();
            uploadedTempProductImages = new();
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