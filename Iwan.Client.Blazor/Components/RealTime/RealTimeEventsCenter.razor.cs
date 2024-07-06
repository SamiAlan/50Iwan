using AKSoftware.Blazor.Utilities;
using Iwan.Client.Blazor.Constants;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.RealTime;
using Iwan.Shared.Extensions;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using MudBlazor;
using System;

namespace Iwan.Client.Blazor.Components.RealTime
{
    public partial class RealTimeEventsCenter
    {
        protected HubConnection _connection;

        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl(NavigationManager.ToAbsoluteUri("/admin-hub"), options =>
                {
                    options.AccessTokenProvider = async () => await LocalStorage.GetItemAsStringAsync(Keys.AccessToken);
                })
                .Build();

            AddCallbacks(_connection);

            await _connection.StartAsync();

            await base.OnInitializedAsync();
        }

        #region Helpers

        private void AddCallbacks(HubConnection connection)
        {
            AddWatermarkingCallbacks(connection);
            AddUnWatermarkingCallbacks(connection);
            AddResizingProductsImagesCallbacks(connection);
            AddResizingCategoriesImagesCallbacks(connection);
            AddResizingCompositionsImagesCallbacks(connection);
            AddResizingSliderImagesCallbacks(connection);
            AddResizingAboutUsImagesCallbacks(connection);
        }

        private void AddWatermarkingCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.WatermarkingJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.WatermarkingJobAdded);
            });

            connection.On<string>(ServerMessages.WatermarkingJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.WatermarkingJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.WatermarkingJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.WatermarkingJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.WatermarkingJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.WatermarkingJobEnded);
            });
        }

        private void AddUnWatermarkingCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.UnWatermarkingJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.UnWatermarkingJobAdded);
            });

            connection.On<string>(ServerMessages.UnWatermarkingJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.UnWatermarkingJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.UnWatermarkingJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.UnWatermarkingJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.UnWatermarkingJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.UnWatermarkingJobEnded);
            });
        }

        private void AddResizingProductsImagesCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.ResizingProductsImagesJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingProductsImagesJobAdded);
            });

            connection.On<string>(ServerMessages.ResizingProductsImagesJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingProductsImagesJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.ResizingProductsImagesJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingProductsImagesJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.ResizingProductsImagesJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingProductsImagesJobEnded);
            });
        }

        private void AddResizingCategoriesImagesCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.ResizingCategoriesImagesJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCategoriesImagesJobAdded);
            });

            connection.On<string>(ServerMessages.ResizingCategoriesImagesJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCategoriesImagesJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.ResizingCategoriesImagesJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCategoriesImagesJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.ResizingCategoriesImagesJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCategoriesImagesJobEnded);
            });
        }

        private void AddResizingCompositionsImagesCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.ResizingCompositionsImagesJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCompositionsImagesJobAdded);
            });

            connection.On<string>(ServerMessages.ResizingCompositionsImagesJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCompositionsImagesJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.ResizingCompositionsImagesJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCompositionsImagesJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.ResizingCompositionsImagesJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingCompositionsImagesJobEnded);
            });
        }

        private void AddResizingSliderImagesCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.ResizingSliderImagesJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingSliderImagesJobAdded);
            });

            connection.On<string>(ServerMessages.ResizingSliderImagesJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingSliderImagesJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.ResizingSliderImagesJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingSliderImagesJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.ResizingSliderImagesJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingSliderImagesJobEnded);
            });
        }

        private void AddResizingAboutUsImagesCallbacks(HubConnection connection)
        {
            connection.On(ServerMessages.ResizingAboutUsImagesJobAdded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingAboutUsImagesJobAdded);
            });

            connection.On<string>(ServerMessages.ResizingAboutUsImagesJobStarted, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingAboutUsImagesJobStarted, data.ToObject<EntityImagesManipulationInitializationDto>());
            });

            connection.On<string>(ServerMessages.ResizingAboutUsImagesJobProgress, data =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingAboutUsImagesJobProgress, data.ToObject<EntityImagesManipulationProgressDto>());
            });

            connection.On(ServerMessages.ResizingAboutUsImagesJobEnded, () =>
            {
                MessagingCenter.Send(this, ServerMessages.ResizingAboutUsImagesJobEnded);
            });
        }

        #endregion
    }
}