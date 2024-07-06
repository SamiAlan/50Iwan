using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Catalog;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Client.Blazor.Services.Vendors;
using Iwan.Shared;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Catalog;
using Iwan.Shared.Options.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Products
{
    public partial class ProductsPage
    {
        protected MudTable<ProductDto> table;
        protected AdminGetProductsOptions options = new() { IncludeMainImage = true };
        protected IEnumerable<CategoryDto> _parentCategories = new List<CategoryDto>();
        protected IEnumerable<CategoryDto> _subCategories = new List<CategoryDto>();
        protected IEnumerable<VendorDto> _vendors = new List<VendorDto>();

        protected string _currentDrawerProductId;
        protected bool _busy;
        protected bool _drawerOpened;

        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IProductService ProductService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected IVendorService VendorService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            options = await GetOrUpdateOptions();

            _parentCategories = await CategoryService.GetCategoriesAsync(new GetAllCategoriesOptions
            {
                IncludeImages = false,
                Type = CategoryType.Parent
            });

            _vendors = await VendorService.GetAllVendorsAsync();

            await base.OnInitializedAsync();
        }

        protected void EditProduct(string productId)
        {
            NavigationManager.NavigateTo(AppPages.EditProduct.ReplaceRouteParameters(productId));
        }

        protected void DeleteProduct(string productId)
        {
            DialogService.ShowConfirmationDialog(
                this, Localize(LocalizeKeys.DeleteProduct), Localize(LocalizeKeys.DeleteProductConfirmation),
                async () => await HandleDeleteProductConfirmedAsync(productId));
        }

        protected void ChangeProductMainImage(ProductDto product)
        {
            DialogService.ShowChangeProductMainImageDialog(this, product.Id, HandleProductMainImageEditedAsync);
        }

        protected void OpenStatesDrawer(string productId)
        {
            _currentDrawerProductId = productId;
            _drawerOpened = true;
        }

        protected async Task HandleDeleteProductConfirmedAsync(string productId)
        {
            _busy = true;

            try
            {
                await ProductService.DeleteProductAsync(productId);
            }
            catch (UnAuthorizedException)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            finally
            {
                _busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.DeleteProductSuccess), Severity.Success);

            await ReloadDataAsync();
        }

        protected async Task HandleProductMainImageEditedAsync()
        {
            Snackbar.Add(Localize(LocalizeKeys.ProductMainImageEdited), Severity.Success);
            await ReloadDataAsync();
        }

        protected void GoToProductImages(string productId)
        {
            NavigationManager.NavigateTo(AppPages.ProductImages.ReplaceRouteParameters(productId));
        }

        protected void GoToProductCategoriesPage(string productId)
        {
            NavigationManager.NavigateTo(AppPages.ProductCategories.ReplaceRouteParameters(productId));
        }

        protected async Task ResizeProductImagesAsync(string productId)
        {
            _busy = true;

            try
            {
                await ProductService.ResizeProductImagesAsync(productId);
                await ReloadDataAsync();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally
            {
                _busy = false;
            }
        }

        protected async Task WatermarkProductImagesAsync(string productId)
        {
            _busy = true;

            try
            {
                await ProductService.WatermarkProductImagesAsync(productId);
                await ReloadDataAsync();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally
            {
                _busy = false;
            }
        }

        protected async Task UnWatermarkProductImagesAsync(string productId)
        {
            _busy = true;

            try
            {
                await ProductService.UnWatermarkProductImagesAsync(productId);
                await ReloadDataAsync();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally
            {
                _busy = false;
            }
        }

        protected async Task FlipProductVisibility(ProductDto product)
        {
            if (_busy) return;

            _busy = true;

            try
            {
                await ProductService.FlipVisibilityAsync(product.Id);

                var productInTable = table.FilteredItems.FirstOrDefault(p => p.Id == product.Id);

                if (productInTable != null) productInTable.IsVisible = !product.IsVisible;
            }
            catch (UnAuthorizedException)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
                return;
            }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }
            finally
            {
                _busy = false;
            }
        }

        protected async Task<TableData<ProductDto>> SearchData(TableState state)
        {
            _busy = true;

            try
            {
                options.CurrentPage = state.Page;
                options.PageSize = state.PageSize;

                if (await LocalStorageService.ContainKeyAsync(nameof(AdminGetProductsOptions)))
                    await SetOptions();

                var data = await ProductService.GetProductsAsync(options);

                return new TableData<ProductDto>
                {
                    TotalItems = data.TotalCount,
                    Items = data.Data
                };
            }
            catch (ServiceException ex) { Snackbar.Add(ex.ErrorMessage, Severity.Error); }
            catch (UnAuthorizedException) { await Logout(); }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }

            finally
            {
                _busy = false;
            }

            return new TableData<ProductDto>
            {
                TotalItems = 0,
                Items = new List<ProductDto>()
            };
        }

        protected async Task ReloadDataAsync()
        {
            await table.ReloadServerData();
        }

        #region Helpers

        private string Localize(string key, params object[] values) => Localizer.Localize(key, values);

        private async Task<AdminGetProductsOptions> GetOrUpdateOptions()
        {
            if (!await LocalStorageService.ContainKeyAsync(nameof(AdminGetProductsOptions)))
                await SetOptions();

            return await LocalStorageService.GetItemAsync<AdminGetProductsOptions>(nameof(AdminGetProductsOptions));

        }

        private async Task SetOptions()
        {
            await LocalStorageService.SetItemAsStringAsync(nameof(AdminGetProductsOptions), options.ToJson());
        }

        protected async Task Logout()
        {
            if (await LocalStorageService.ContainKeyAsync(Keys.AccessToken))
                await LocalStorageService.RemoveItemAsync(Keys.AccessToken);

            if (await LocalStorageService.ContainKeyAsync(Keys.RefreshToken))
                await LocalStorageService.RemoveItemAsync(Keys.RefreshToken);

            await AuthStateProvider.GetAuthenticationStateAsync();

            NavigationManager.NavigateTo(AppPages.Login);
        }

        #endregion
    }
}