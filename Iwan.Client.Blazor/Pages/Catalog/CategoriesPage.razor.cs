using Blazored.LocalStorage;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Catalog;
using Iwan.Shared;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Catalog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Catalog
{
    public partial class CategoriesPage
    {
        protected MudTable<CategoryDto> table;
        protected GetCategoriesOptions options = new() { IncludeImages = true };
        protected IEnumerable<CategoryDto> _parentCategories = new List<CategoryDto>();
        protected bool busy;

        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected IDialogService DialogService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            options = await GetOrUpdateOptions();

            _parentCategories = await CategoryService.GetCategoriesAsync(new GetAllCategoriesOptions
            {
                Type = CategoryType.Parent
            });

            await base.OnInitializedAsync();
        }

        protected void EditCategory(string categoryId)
        {
            NavigationManager.NavigateTo(AppPages.EditCategory.ReplaceRouteParameters(categoryId));
        }

        protected void DeleteCategory(string categoryId)
        {
            DialogService.ShowConfirmationDialog(
                this, Localize(LocalizeKeys.DeleteCategory), Localize(LocalizeKeys.DeleteCategoryConfirmation),
                async () => await HandleDeleteCategoryConfirmedAsync(categoryId));
        }

        protected void ChangeCategoryImage(CategoryDto category)
        {
            DialogService.ShowChangeCategoryImageDialog(this, category.Id, HandleCategoryImageChangedAsync);
        }

        protected async Task HandleDeleteCategoryConfirmedAsync(string categoryId)
        {
            busy = true;

            try
            {
                await CategoryService.DeleteCategoryAsync(categoryId);
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
            finally
            {
                busy = false;
            }

            Snackbar.Add(Localize(LocalizeKeys.DeleteCategorySuccess), Severity.Success);

            await ReloadDataAsync();
        }

        protected async Task HandleCategoryImageChangedAsync()
        {
            Snackbar.Add(Localize(LocalizeKeys.CategoryImageChanged), Severity.Success);
            await ReloadDataAsync();
        }

        protected async Task FlipCategoryVisibility(CategoryDto category)
        {
            if (busy) return;

            busy = true;

            try
            {
                await CategoryService.FlipVisibilityAsync(category.Id);
                category.IsVisible = !category.IsVisible;

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
                busy = false;
            }
        }

        protected async Task<TableData<CategoryDto>> SearchData(TableState state)
        {
            busy = true;

            try
            {
                options.CurrentPage = state.Page;
                options.PageSize = state.PageSize;
                await SetOptions();

                var data = await CategoryService.GetCategoriesAsync(options);

                return new TableData<CategoryDto>
                {
                    TotalItems = data.TotalCount,
                    Items = data.Data
                };
            }
            catch (UnAuthorizedException ex) { await Logout(); }
            catch (Exception ex) { Snackbar.Add(ex.Message, Severity.Error); }

            finally
            {
                busy = false;
            }

            return new TableData<CategoryDto>
            {
                TotalItems = 0,
                Items = new List<CategoryDto>()
            };
        }

        protected async Task ReloadDataAsync() => await table.ReloadServerData();

        #region Helpers

        private string Localize(string key, params object[] values) => Localizer.Localize(key, values);

        private async Task<GetCategoriesOptions> GetOrUpdateOptions()
        {
            if (!await LocalStorageService.ContainKeyAsync(nameof(GetCategoriesOptions)))
                await LocalStorageService.SetItemAsync(nameof(GetCategoriesOptions), options);

            return await LocalStorageService.GetItemAsync<GetCategoriesOptions>(nameof(GetCategoriesOptions));

        }

        private async Task SetOptions()
        {
            await LocalStorageService.SetItemAsStringAsync(nameof(GetCategoriesOptions), options.ToJson());
        }

        #endregion

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
