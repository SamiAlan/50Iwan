using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Settings;
using Iwan.Shared;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Settings
{
    public partial class WatermarkImageEditor
    {
        protected bool busy;
        protected TempImageDto _tempImageFromWatermarkImage;

        [Parameter] public WatermarkImageDto Image { get; set; }
        [Inject] ISettingsService SettingsService { get; set; }
        [Inject] IStringLocalizer<Localization> Localizer { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Image != null)
                _tempImageFromWatermarkImage = new TempImageDto
                {
                    SmallImageUrl = Image.Image.Url
                };
        }

        protected async Task ChangeImageAsync(TempImageDto image)
        {
            busy = true;

            try
            {
                Image = await SettingsService.ChangeWatermarkImageAsync(new ChangeWatermarkImageDto
                {
                    Image = new(image.Id)
                });

                _tempImageFromWatermarkImage = new TempImageDto
                {
                    SmallImageUrl = Image.Image.Url
                };
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
            }
            catch(ServiceException e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}