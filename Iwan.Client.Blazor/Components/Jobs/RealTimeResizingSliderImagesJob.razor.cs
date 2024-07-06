using AKSoftware.Blazor.Utilities;
using Iwan.Client.Blazor.Components.RealTime;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.RealTime;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;

namespace Iwan.Client.Blazor.Components.Jobs
{
    public partial class RealTimeResizingSliderImagesJob : IDisposable
    {
        protected bool _shouldShowProgress;
        protected JobStatus _status;
        protected int _totalEntities;
        protected int _entitiesDone;
        protected int _entitiesLeft;

        [Inject] IStringLocalizer<Localization> Localizer { get; set; }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingSliderImagesJobAdded);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.ResizingSliderImagesJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.ResizingSliderImagesJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingSliderImagesJobStarted);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingSliderImagesJobAdded, (_) => ResizingSliderImagesJobAdded());
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.ResizingSliderImagesJobStarted, (s, d) => ResizingSliderImagesJobStarted(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.ResizingSliderImagesJobProgress, (s, d) => ResizingSliderImagesJobProgressed(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingSliderImagesJobEnded, (_) => ResizingSliderImagesJobEnded());
        }

        protected void ResizingSliderImagesJobAdded()
        {
            _shouldShowProgress = true;
            _status = JobStatus.Pending;
            StateHasChanged();
        }

        protected void ResizingSliderImagesJobEnded()
        {
            _shouldShowProgress = false;
            _status = JobStatus.Ended;
            _totalEntities = 0;
            _entitiesDone = 0;
            _entitiesLeft = 0;
            StateHasChanged();
        }

        protected void ResizingSliderImagesJobStarted(EntityImagesManipulationInitializationDto initializationData)
        {
            _shouldShowProgress = true;
            _status = JobStatus.Processing;
            _totalEntities = initializationData.NumberOfEntitiesToProcess;
            StateHasChanged();
        }

        protected void ResizingSliderImagesJobProgressed(EntityImagesManipulationProgressDto progressData)
        {
            _shouldShowProgress = true;
            _status = JobStatus.Processing;
            _totalEntities = progressData.NumberOfEntitiesToProcess;
            _entitiesDone = progressData.DoneEntities;
            _entitiesLeft = progressData.EntitiesLeft;
            StateHasChanged();
        }

        protected string Localize(string key, params string[] values) => Localizer.Localize(key, values);
    }
}