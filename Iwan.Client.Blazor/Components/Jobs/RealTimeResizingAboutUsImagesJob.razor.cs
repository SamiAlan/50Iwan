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
    public partial class RealTimeResizingAboutUsImagesJob
    {
        protected bool _shouldShowProgress;
        protected JobStatus _status;
        protected int _totalEntities;
        protected int _entitiesDone;
        protected int _entitiesLeft;

        [Inject] IStringLocalizer<Localization> Localizer { get; set; }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingAboutUsImagesJobAdded);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.ResizingAboutUsImagesJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.ResizingAboutUsImagesJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingAboutUsImagesJobStarted);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingAboutUsImagesJobAdded, (_) => ResizingAboutUsImagesJobAdded());
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.ResizingAboutUsImagesJobStarted, (s, d) => ResizingAboutUsImagesJobStarted(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.ResizingAboutUsImagesJobProgress, (s, d) => ResizingAboutUsImagesJobProgressed(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingAboutUsImagesJobEnded, (_) => ResizingAboutUsImagesJobEnded());
        }

        protected void ResizingAboutUsImagesJobAdded()
        {
            _shouldShowProgress = true;
            _status = JobStatus.Pending;
            StateHasChanged();
        }

        protected void ResizingAboutUsImagesJobEnded()
        {
            _shouldShowProgress = false;
            _status = JobStatus.Ended;
            _totalEntities = 0;
            _entitiesDone = 0;
            _entitiesLeft = 0;
            StateHasChanged();
        }

        protected void ResizingAboutUsImagesJobStarted(EntityImagesManipulationInitializationDto initializationData)
        {
            _shouldShowProgress = true;
            _status = JobStatus.Processing;
            _totalEntities = initializationData.NumberOfEntitiesToProcess;
            StateHasChanged();
        }

        protected void ResizingAboutUsImagesJobProgressed(EntityImagesManipulationProgressDto progressData)
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