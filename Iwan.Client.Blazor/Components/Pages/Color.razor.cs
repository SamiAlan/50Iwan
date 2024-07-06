using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Pages;
using Iwan.Shared;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Pages
{
    public partial class Color
    {
        protected bool _busy;

        [Parameter] public ColorDto ColorDto { get; set; }
        [Parameter] public EventCallback<ColorDto> OnColorDeleted { get; set; }
        [Inject] IPagesService PagesService { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected async Task DeleteColorAsync()
        {
            if (_busy) return;

            _busy = true;

            try
            {
                await PagesService.DeleteColorAsync(ColorDto.Id);

                if (OnColorDeleted.HasDelegate)
                    await OnColorDeleted.InvokeAsync(ColorDto);
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

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}