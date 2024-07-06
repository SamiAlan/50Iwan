using Iwan.Client.Blazor.Components.Catalog;
using Iwan.Client.Blazor.Components.Common;
using Iwan.Client.Blazor.Components.Compositions;
using Iwan.Client.Blazor.Components.Products;
using Iwan.Client.Blazor.Components.SliderImages;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Dtos.Sliders;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Extensions
{
    public static class DialogServiceExtensions
    {
        public static void ShowConfirmationDialog(this IDialogService service, ComponentBase sourceComponent, string title = null, string message = null, Func<Task> confirmationAction = default, Func<Task> cancellationAction = default)
        {
            // Show delete confirmation
            var parameters = new DialogParameters
            {
                { "Message", message }
            };

            if (confirmationAction != null)
                parameters.Add("OnConfirmClicked", EventCallback.Factory.Create(sourceComponent, confirmationAction));

            if (cancellationAction != null)
                parameters.Add("OnCancelClicked", EventCallback.Factory.Create(sourceComponent, cancellationAction));

            var options = new DialogOptions
            {
                DisableBackdropClick = true
            };

            service.Show<ConfirmationDialog>(title, parameters, options);
        }

        public static void ShowChangeCategoryImageDialog(this IDialogService service, ComponentBase sourceComponent, string categoryId, Func<Task> onImageChanged = null)
        {
            // Show delete confirmation
            var parameters = new DialogParameters()
            {
                { "CategoryId", categoryId }
            };

            if (onImageChanged != null)
                parameters.Add("OnImageChanged", EventCallback.Factory.Create(sourceComponent, onImageChanged));

            service.Show<ChangeCategoryImageDialog>(string.Empty, parameters);
        }

        public static void ShowChangeCompositionImageDialog(this IDialogService service, ComponentBase sourceComponent, string compositionId, Func<Task> onImageChanged = null)
        {
            // Show delete confirmation
            var parameters = new DialogParameters()
            {
                { "CompositionId", compositionId }
            };

            if (onImageChanged != null)
                parameters.Add("OnImageChanged", EventCallback.Factory.Create(sourceComponent, onImageChanged));

            service.Show<ChangeCompositionImageDialog>(string.Empty, parameters);
        }

        public static void ShowAddSliderImageDialog(this IDialogService service, ComponentBase sourceComponent, Func<Task> onSliderImageAdded = null)
        {
            // Show delete confirmation
            var parameters = new DialogParameters();

            if (onSliderImageAdded != null)
                parameters.Add("SliderImageAdded", EventCallback.Factory.Create(sourceComponent, onSliderImageAdded));

            service.Show<AddSliderImageDialog>(string.Empty, parameters);
        }

        public static void ShowEditSliderImageDialog(this IDialogService service, ComponentBase sourceComponent, EditSliderImageDto sliderImage, Func<Task> onSliderImageEdited = null)
        {
            // Show delete confirmation
            var parameters = new DialogParameters()
            {
                { "SliderImage", sliderImage }
            };

            if (onSliderImageEdited != null)
                parameters.Add("SliderImageEdited", EventCallback.Factory.Create(sourceComponent, onSliderImageEdited));

            service.Show<EditSliderImageDialog>(string.Empty, parameters);
        }

        public static void ShowChangeProductMainImageDialog(this IDialogService service, ComponentBase sourceComponent, string productId, Func<Task> onImageChanged = null)
        {
            // Show delete confirmation
            var parameters = new DialogParameters()
            {
                { "ProductId", productId }
            };

            if (onImageChanged != null)
                parameters.Add("OnImageChanged", EventCallback.Factory.Create(sourceComponent, onImageChanged));

            service.Show<ChangeProductMainImageDialog>(string.Empty, parameters);
        }
    }
}
