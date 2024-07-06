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
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Pages
{
    public partial class InteriorDesignImageEditor
    {
        protected bool busy;
        protected bool hovering;
        protected InteriorDesignSectionImageDto image;
        protected TempImageDto tempMainImageFromInteriorImage;
        protected TempImageDto tempMobileImageFromInteriorImage;
        
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] IPagesService PagesService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Parameter] public InteriorDesignSectionImageDto Image { get; set; }

        protected override void OnParametersSet()
        {
            if (Image != null)
            {
                image = Image;

                if (image.MainImage != null)
                    tempMainImageFromInteriorImage = new TempImageDto
                    {
                        SmallImageUrl = image.MainImage.Url
                    };

                if (image.MobileImage != null)
                    tempMobileImageFromInteriorImage = new TempImageDto
                    {
                        SmallImageUrl = image.MobileImage.Url
                    };
            }
            base.OnParametersSet();
        }

        protected async Task ChangeMainImageAsync(TempImageDto tempImage)
        {
            busy = true;

            try
            {
                image = await PagesService.ChangeMainImageAsync(new(new(tempImage.Id)));
                tempMainImageFromInteriorImage = new TempImageDto
                {
                    SmallImageUrl = image.MainImage.Url
                };
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

        protected async Task ChangeMobileImageAsync(TempImageDto tempImage)
        {
            busy = true;

            try
            {
                image = await PagesService.ChangeMobileImageAsync(new(new(tempImage.Id)));
                tempMobileImageFromInteriorImage = new TempImageDto
                {
                    SmallImageUrl = image.MobileImage.Url
                };
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

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}