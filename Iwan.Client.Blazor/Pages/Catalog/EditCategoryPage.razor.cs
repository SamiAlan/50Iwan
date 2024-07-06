using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Catalog;
using Iwan.Shared;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Catalog;
using Iwan.Shared.Validators.Catalog.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Catalog
{
    public partial class EditCategoryPage
    {
        #region Validators

        protected EditCategoryDtoValidator _categoryValidator;

        #endregion

        protected EditCategoryDto category = new();
        protected bool busy = false;
        protected ServerValidation _serverValidator;
        protected IEnumerable<CategoryDto> _parentCategories;

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Parameter] public string Id { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            _categoryValidator = new EditCategoryDtoValidator(Localizer);
            _parentCategories = await CategoryService.GetCategoriesAsync(new GetAllCategoriesOptions
            {
                Type = CategoryType.Parent
            });

            await GetCategoryInfoAsync(Id);

            await base.OnInitializedAsync();
        }

        protected async Task EditCategoryAsync()
        {
            busy = true;

            try
            {
                await CategoryService.EditCategoryAsync(category);
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
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(category));

                return;
            }
            finally { busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.CategoryEdited), Severity.Success);
        }

        protected async Task GetCategoryInfoAsync(string id)
        {
            var categoryInfo = await CategoryService.GetCategoryByIdAsync(id);

            if (categoryInfo == null)
            {
                GoToCategoriesPage();
            }
            else
            {
                category = categoryInfo.MapToEditCategoryDto();
            }
        }

        protected void GoToCategoriesPage() => NavigationManager.NavigateTo(AppPages.Categories);

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
