using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Pages;
using Iwan.Shared;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Pages
{
    public partial class AboutUsSectionImagesEditor
    {
        protected bool busy;
        protected List<AboutUsSectionImageDto> _aboutUsImages = new();

        [Parameter] public List<AboutUsSectionImageDto> Images { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IPagesService PagesService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected override void OnParametersSet()
        {
            if (Images != null) _aboutUsImages = Images;

            base.OnParametersSet();
        }

        protected async Task AddAboutUsImageAsync(TempImageDto tempImage)
        {
            busy = true;

            try
            {
                var aboutUsImage = await PagesService.AddImageAsync(new(new AddImageDto(tempImage.Id)));

                _aboutUsImages.Add(aboutUsImage);
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
                busy = false;
            }
        }

        protected void OnImageDeleted(AboutUsSectionImageDto image)
        {
            _aboutUsImages.Remove(image);
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}