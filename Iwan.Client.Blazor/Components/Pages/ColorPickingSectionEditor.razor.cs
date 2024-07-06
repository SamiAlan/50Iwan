using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Pages;
using Iwan.Shared;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Pages
{
    public partial class ColorPickingSectionEditor
    {
        protected bool _busy;
        protected AddColorDto color = new();
        protected AddColorDtoValidator _validator;

        [Parameter] public IList<ColorDto> Colors { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IPagesService PagesService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _validator = new(Localizer);
        }

        private string Localize(string key, params string[] values) => Localizer.Localize(key, values);

        protected async Task AddColorAsync()
        {
            _busy = true;

            try
            {
                var addedColor = await PagesService.AddColorAsync(color);
                Colors.Add(addedColor);
                color = new();
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

        protected void ColorDeleted(ColorDto deletedColor)
        {
            Colors.Remove(deletedColor);
        }
    }
}