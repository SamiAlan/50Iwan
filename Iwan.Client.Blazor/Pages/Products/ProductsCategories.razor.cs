using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Catalog;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Shared;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Catalog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Products
{
    public partial class ProductsCategories
    {
        bool busy;
        string chosenCategoriesSearchText = string.Empty;
        string unchosenCategoriesSearchText = string.Empty;
        List<ProductCategoryDto> productCategories = new();
        List<CategoryDto> unchosenCategories = new();

        List<ProductCategoryDto> ProductCategories
        {
            get
            {
                if (!chosenCategoriesSearchText.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    return productCategories
                        .Where(c => c.ArabicName.ToLower().Contains(chosenCategoriesSearchText.ToLower()) ||
                            c.EnglishName.ToLower().Contains(chosenCategoriesSearchText.ToLower())).ToList();
                }

                return productCategories;
            }
            set
            {
                if (productCategories != value) productCategories = value;
            }
        }
        List<CategoryDto> UnchosenCategories
        {
            get
            {
                if (!unchosenCategoriesSearchText.IsNullOrEmptyOrWhiteSpaceSafe())
                {
                    return unchosenCategories
                        .Where(c => c.ArabicName.ToLower().Contains(unchosenCategoriesSearchText.ToLower()) ||
                            c.EnglishName.ToLower().Contains(unchosenCategoriesSearchText.ToLower())).ToList();
                }

                return unchosenCategories;
            }
            set
            {
                if (unchosenCategories != value) unchosenCategories = value;
            }
        }

        [Parameter] public string Id { get; set; }
        [Inject] IProductService ProductService { get; set; }
        [Inject] ICategoryService CategoryService { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            busy = true;

            ProductCategories = (await ProductService.GetProductCategoriesAsync(Id)).ToList();
            UnchosenCategories = (await CategoryService.GetCategoriesAsync(new GetAllCategoriesOptions
            {
                IncludeImages = false,
            })).ToList();

            IntersectCategories();

            busy = false;
            await base.OnInitializedAsync();
        }

        private void IntersectCategories()
        {
            if (!productCategories.Any()) return;

            busy = true;
            var chosenCategoriesIds = productCategories.Select(c => c.CategoryId);
            
            UnchosenCategories = UnchosenCategories.Where(c => !chosenCategoriesIds.Contains(c.Id)).ToList();
            
            busy = false;
        }

        private async Task AddCategoryToProduct(CategoryDto category)
        {
            busy = true;

            try
            {
                var addedProductCategory = await ProductService.AddProductCategoryAsync(new AddProductCategoryDto
                {
                    ProductId = Id,
                    CategoryId = category.Id
                });

                productCategories.Add(addedProductCategory);
                unchosenCategories.Remove(category);
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(Localize(e.ErrorMessage), Severity.Error);
                return;
            }
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.CategoryAddedToProduct), Severity.Success);
        }

        private async Task RemoveCategoryFromProductAsync(ProductCategoryDto productCategory)
        {
            busy = true;

            try
            {
                await ProductService.DeleteProductCategoryAsync(productCategory.Id);

                productCategories.Remove(productCategory);
                unchosenCategories.Add(new CategoryDto
                {
                    Id = productCategory.CategoryId,
                    ArabicName = productCategory.ArabicName,
                    EnglishName = productCategory.EnglishName
                });
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                Snackbar.Add(Localize(e.ErrorMessage), Severity.Error);
                return;
            }
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.CategoryRemovedFromProduct), Severity.Success);
        }

        private void GoToProductsPage()
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