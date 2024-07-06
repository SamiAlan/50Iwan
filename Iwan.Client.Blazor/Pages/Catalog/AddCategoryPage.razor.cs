using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Catalog;
using Iwan.Client.Blazor.Services.Media;
using Iwan.Shared;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Dtos.Media;
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
    public partial class AddCategoryPage
    {
        #region Validators

        protected AddCategoryDtoValidator _validator;

        #endregion

        protected AddCategoryDto category = new();
        protected bool busy = false;
        protected ServerValidation _serverValidator;
        protected IEnumerable<CategoryDto> _parentCategories = new List<CategoryDto>();

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected IImageService ImageService { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            _validator = new AddCategoryDtoValidator(Localizer);
            _parentCategories = await CategoryService.GetCategoriesAsync(new GetAllCategoriesOptions
            {
                Type = CategoryType.Parent
            });

            await base.OnInitializedAsync();
        }

        protected async Task AddCategoryAsync()
        {
            busy = true;

            try
            {
                await CategoryService.AddCategoryAsync(category);
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

            Snackbar.Add(Localize(LocalizeKeys.CategoryAdded), Severity.Success);
            Reset();
        }

        public void OnUploadingTempImage()
        {
            busy = true;
        }

        public void OnTempImageDeleted()
        {
            category.Image = null;
        }

        public void OnTempImageUploaded(TempImageDto tempImageDto)
        {
            busy = false;
            category.Image = new AddCategoryImageDto(new AddImageDto(tempImageDto.Id));
        }

        protected void GoToCategoriesPage() => NavigationManager.NavigateTo(AppPages.Categories);

        protected void Reset()
        {
            category = new();
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