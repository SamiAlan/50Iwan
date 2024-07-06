using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Products;
using Iwan.Shared;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Validators.Products.Admin;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Extensions;

namespace Iwan.Client.Blazor.Components.Products
{
    public partial class ProductStatesEditor
    {
        protected bool _busy;
        protected List<ProductStateDto> _existingStates = new();
        protected List<AddProductStateDto> _newStates = new();
        protected AddProductStateDto _newState = new();

        protected AddProductStateDtoValidator _validator;
        protected ServerValidation _serverValidator;

        [Parameter] public string ProductId { get; set; }
        [Inject] IProductService ProductService { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        public List<AddProductStateDto> TempStates => _newStates;
        public bool RelatedToProduct { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _validator = new(Localizer);

            RelatedToProduct = !ProductId.IsNullOrEmptyOrWhiteSpaceSafe();

            if (RelatedToProduct)
                await GetStatesAsync();

            await base.OnInitializedAsync();
        }

        protected async Task GetStatesAsync()
        {
            _busy = true;

            try
            {
                _existingStates = (await ProductService.GetProductStatesAsync(ProductId)).ToList();
                System.Console.WriteLine(_existingStates.Count);
                System.Console.WriteLine(RelatedToProduct);
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
            }
            finally
            {
                _busy = false;
            }
        }
        
        protected async Task AddStateToExistingProductAsync()
        {
            _busy = true;
            _newState.ProductId = ProductId;

            try
            {
                var addedState = await ProductService.AddProductStateAsync(_newState);
                _existingStates.Add(addedState);
                _newState = new();
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            catch (ServiceException e)
            {
                if (!e.Errors.Any())
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
                else
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(_newState));
            }
            finally { _busy = false; }
        }

        protected async Task DeleteExistingStateAsync(ProductStateDto state)
        {
            _busy = true;

            try
            {
                await ProductService.DeleteProductStateAsync(state.Id);
                _existingStates.Remove(state);
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            catch (ServiceException e)
            {
                Snackbar.Add(e.ErrorMessage, Severity.Error);
            }
            finally { _busy = false; }
        }

        protected void AddTempState()
        {
            _newState.ProductId = ProductId;
            _newStates.Add(_newState);
            _newState = new();
        }

        protected void DeleteTempState(AddProductStateDto state)
        {
            _newStates.Remove(state);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}