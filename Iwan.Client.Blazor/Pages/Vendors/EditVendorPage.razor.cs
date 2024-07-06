using Blazored.LocalStorage;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Common;
using Iwan.Client.Blazor.Services.Vendors;
using Iwan.Shared;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Common;
using Iwan.Shared.Validators.Vendors.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Vendors
{
    public partial class EditVendorPage
    {
        #region Validators

        protected EditVendorDtoValidator _vendorValidator;
        protected EditAddressDtoValidator _addressValidator;

        #endregion

        protected EditVendorDto vendor = new();
        protected EditAddressDto address = new();
        protected ServerValidation _vendorServerValidator;
        protected ServerValidation _addressServerValidator;
        protected bool busy = false;

        [Parameter] public string Id { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] protected IVendorService VendorService { get; set; }
        [Inject] protected IAddressService AddressService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (!(authState.User?.Identity?.IsAuthenticated ?? false))
                await Logout();

            await GetVendorDetailsAsync();

            _vendorValidator = new EditVendorDtoValidator(Localizer);
            _addressValidator = new EditAddressDtoValidator(Localizer);
            
            await base.OnInitializedAsync();
        }

        protected async Task EditAddressAsync()
        {
            busy = true;

            try
            {
                var addressrDto = await AddressService.EditAddressAsync(address);
                address = addressrDto.MapToEditAddressDto();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                if (e.Errors.Any())
                    _addressServerValidator.DisplayErrors(e.Errors.ToProperValidationObject(address));
                else
                    Snackbar.Add(e.ErrorMessage, Severity.Error);

                return;
            }
            finally { busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.AddressEdited), Severity.Success);
        }

        protected async Task EditVendorAsync()
        {
            busy = true;

            try
            {
                var vendorDto = await VendorService.EditVendorAsync(vendor);
                vendor = vendorDto.MapToEditVendorDto();
            }
            catch (UnAuthorizedException e)
            {
                await Logout();
                return;
            }
            catch (ServiceException e)
            {
                if (e.Errors.Any())
                    _vendorServerValidator.DisplayErrors(e.Errors.ToProperValidationObject(vendor));
                else
                    Snackbar.Add(e.ErrorMessage, Severity.Error);

                return;
            }
            finally { busy = false; }

            Snackbar.Add(Localize(LocalizeKeys.VendorEdited), Severity.Success);
        }

        protected async Task GetVendorDetailsAsync()
        {
            try
            {
                var vendorDto = await VendorService.GetVendorAsync(Id);

                vendor = new EditVendorDto
                {
                    Id = vendorDto.Id,
                    Name = vendorDto.Name,
                    BenefitPercent = vendorDto.BenefitPercent,
                    ShowOwnProducts = vendorDto.ShowOwnProducts
                };

                address = new EditAddressDto
                {
                    Id = vendorDto.Address.Id,
                    City = vendorDto.Address.City,
                    Company = vendorDto.Address.Company,
                    Country = vendorDto.Address.Country,
                    Email = vendorDto.Address.Email,
                    PhoneNumber = vendorDto.Address.PhoneNumber,
                    Address1 = vendorDto.Address.Address1,
                    Address2 = vendorDto.Address.Address2
                };
            }
            catch
            {
                Snackbar.Add(Localize(LocalizeKeys.Error), Severity.Error, options => options.VisibleStateDuration = 2);
                await Task.Delay(2000);
                NavigationManager.NavigateTo(AppPages.Vendors);
                return;
            }
        }

        protected void GoToVendorsPage() => NavigationManager.NavigateTo(AppPages.Vendors);

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
