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
    public partial class AboutUsImage
    {
        protected bool hovering;
        protected bool deletingImage;

        [Parameter] public EventCallback<AboutUsSectionImageDto> ImageDeleted { get; set; }
        [Parameter] public EventCallback<string> ErrorOccured { get; set; }
        [Parameter] public AboutUsSectionImageDto Image { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IPagesService PagesService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected async Task DeleteImageAsync()
        {
            if (deletingImage) return;

            deletingImage = true;

            try
            {
                await PagesService.DeleteAboutUsImageAsync(Image.Id);

                if (ImageDeleted.HasDelegate)
                    await ImageDeleted.InvokeAsync(Image);
            }
            catch (UnAuthorizedException e)
            {
                if (ErrorOccured.HasDelegate)
                    await ErrorOccured.InvokeAsync(Localize(e.Message));
                else
                    Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            catch (ServiceException e)
            {
                if (ErrorOccured.HasDelegate)
                    await ErrorOccured.InvokeAsync(Localize(e.Message));
                else
                    Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            finally
            {
                deletingImage = false;
                hovering = false;
            }
        }

        private string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}