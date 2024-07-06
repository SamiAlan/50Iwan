using Iwan.Client.Blazor.Components.Validation;
using Iwan.Client.Blazor.Extensions;
using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.SliderImages;
using Iwan.Shared;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Sliders.Admin;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.SliderImages
{
    public partial class AddSliderImageDialog
    {
        protected bool busy = false;
        protected AddSliderImageDto sliderImage = new();
        protected AddSliderImageDtoValidator _imageValidator;
        protected ServerValidation _serverValidator;
        [CascadingParameter]
        MudDialogInstance DialogInstance { get; set; }

        [Parameter]
        public EventCallback SliderImageAdded { get; set; }

        [Inject]
        public IStringLocalizer<Localization> Localizer { get; set; }

        [Inject]
        public ISliderImageService SliderImageService { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _imageValidator = new(Localizer);
        }

        protected async Task AddImageAsync()
        {
            busy = true;
            try
            {
                await SliderImageService.AddSliderImageAsync(sliderImage);
                if (SliderImageAdded.HasDelegate)
                    await SliderImageAdded.InvokeAsync();
                DialogInstance.Close(DialogResult.Ok(true));
            }
            catch (UnAuthorizedException e)
            {
                Snackbar.Add(Localize(e.Message), Severity.Error);
                return;
            }
            catch (ServiceException e)
            {
                if (!e.Errors.Any())
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
                else
                    _serverValidator.DisplayErrors(e.Errors.ToProperValidationObject(sliderImage));
                return;
            }
            finally
            {
                busy = false;
            }
        }

        protected void OnTempImageDeleted()
        {
            sliderImage.Image = null;
        }

        protected void OnTempImageUploaded(TempImageDto tempImage)
        {
            sliderImage.Image = new(tempImage.Id);
            busy = false;
        }

        protected void OnUploadingTempImage()
        {
            busy = true;
        }

        protected void HandleCancelClicked()
        {
            DialogInstance.Close(DialogResult.Ok(false));
        }

        protected string Localize(string key, params object[] values) => Localizer.Localize(key, values);
    }
}