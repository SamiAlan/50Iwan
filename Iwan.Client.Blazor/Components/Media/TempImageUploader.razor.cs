using Iwan.Client.Blazor.Infrastructure.Exceptions;
using Iwan.Client.Blazor.Services.Media;
using Iwan.Shared;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Components.Media
{
    public partial class TempImageUploader<TValidationFor>
    {
        private readonly string inputFileId = Guid.NewGuid().ToString();
        private bool uploadingTempImage = false;
        private bool deletingImage = false;
        private TempImageDto uploadedTempImage;
        private bool hovering = false;
        private double uploadProgress;

#nullable enable
        [Parameter]
        public Expression<Func<TValidationFor>>? ValidationFor { get; set; }
#nullable disable

        [Parameter]
        public EventCallback UploadingTempImage { get; set; }

        [Parameter]
        public EventCallback<TempImageDto> TempImageUploaded { get; set; }

        [Parameter]
        public EventCallback<TempImageDto> TempImageDeleted { get; set; }

        [Parameter]
        public EventCallback<string> ErrorOccured { get; set; }

        [Parameter]
        public TempImageDto UploadedTempImage { get; set; }

        [Parameter]
        public bool ResetAfterUpload { get; set; }

        [Parameter]
        public bool ParentBusy { get; set; }

        [Parameter]
        public bool CanUploadAfterUploaded { get; set; } = true;

        [Parameter]
        public bool CanDeleteAfterUploaded { get; set; } = true;

        [Parameter]
        public bool AddLightGrayBackground { get; set; } = false;

        [Inject]
        ISnackbar Snackbar { get; set; }

        [Inject]
        IImageService ImageService { get; set; }

        [Inject]
        IStringLocalizer<Localization> Localizer { get; set; }

        protected override void OnInitialized()
        {
            if (UploadedTempImage != null)
                uploadedTempImage = UploadedTempImage;

            base.OnInitialized();
        }

        protected async Task UploadTempImageAsync(InputFileChangeEventArgs args)
        {
            if (uploadingTempImage) return;

            var image = args.File;
            uploadingTempImage = true;

            if (UploadingTempImage.HasDelegate)
                await UploadingTempImage.InvokeAsync();
            try
            {
                uploadedTempImage = await ImageService.UploadTempImageAsync(image.OpenReadStream(50000000), image.Name, (u, p) =>
                {
                    uploadProgress = p;
                    StateHasChanged();
                });

                if (TempImageUploaded.HasDelegate)
                    await TempImageUploaded.InvokeAsync(uploadedTempImage);

                if (ResetAfterUpload)
                    Reset();
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
                    await ErrorOccured.InvokeAsync(e.ErrorMessage);
                else
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
            }
            catch
            {
                uploadedTempImage = null;
            }
            finally
            {
                uploadProgress = 0;
                uploadingTempImage = false;
                hovering = false;
            }
        }

        protected async Task DeleteTempImageAsync(bool shouldInformOnDelete = true)
        {
            if (deletingImage) return;

            deletingImage = true;

            try
            {
                if (uploadedTempImage != null)
                {
                    await ImageService.DeleteTempImageAsync(uploadedTempImage.Id);
                    if (shouldInformOnDelete && TempImageDeleted.HasDelegate)
                        await TempImageDeleted.InvokeAsync(uploadedTempImage);

                    uploadedTempImage = null;
                }
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
                    await ErrorOccured.InvokeAsync(e.ErrorMessage);
                else
                    Snackbar.Add(e.ErrorMessage, Severity.Error);
            }
            finally
            {
                deletingImage = false;
            }
        }

        private void Reset()
        {
            deletingImage = false;
            uploadingTempImage = false;
            hovering = false;
            uploadedTempImage = null;
        }

        private string Localize(string key, params object[] values) => Localizer.Localize(key, values);
    }
}