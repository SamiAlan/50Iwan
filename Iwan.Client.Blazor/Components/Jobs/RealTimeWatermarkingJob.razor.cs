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
    public partial class RealTimeWatermarkingJob : IDisposable
    {
        protected bool _shouldShowProgress;
        protected JobStatus _status;
        protected int _totalEntities;
        protected int _entitiesDone;
        protected int _entitiesLeft;

        [Inject] IStringLocalizer<Localization> Localizer { get; set; }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.WatermarkingJobAdded);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.WatermarkingJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.WatermarkingJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.WatermarkingJobStarted);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.WatermarkingJobAdded, (_) => WatermarkingJobAdded());
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.WatermarkingJobStarted,(s, d) => WatermarkingJobStarted(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.WatermarkingJobProgress,(s, d) => WatermarkingJobProgressed(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.WatermarkingJobEnded,(_) => WatermarkingJobEnded());
        }

        protected void WatermarkingJobAdded()
        {
            _shouldShowProgress = true;
            _status = JobStatus.Pending;
            StateHasChanged();
        }

        protected void WatermarkingJobEnded()
        {
            _shouldShowProgress = false;
            _status = JobStatus.Ended;
            _totalEntities = 0;
            _entitiesDone = 0;
            _entitiesLeft = 0;
            StateHasChanged();
        }

        protected void WatermarkingJobStarted(EntityImagesManipulationInitializationDto initializationData)
        {
            _shouldShowProgress = true;
            _status = JobStatus.Processing;
            _totalEntities = initializationData.NumberOfEntitiesToProcess;
            StateHasChanged();
        }

        protected void WatermarkingJobProgressed(EntityImagesManipulationProgressDto progressData)
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