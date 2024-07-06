using AKSoftware.Blazor.Utilities;
using Iwan.Client.Blazor.Components.RealTime;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.RealTime;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Iwan.Client.Blazor.Components.Jobs
{
    public partial class RealTimeResizingCategoriesImagesJob
    {
        protected bool _shouldShowProgress;
        protected JobStatus _status;
        protected int _totalEntities;
        protected int _entitiesDone;
        protected int _entitiesLeft;

        [Inject] IStringLocalizer<Localization> Localizer { get; set; }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingCategoriesImagesJobAdded);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.ResizingCategoriesImagesJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.ResizingCategoriesImagesJobStarted);
            MessagingCenter.Unsubscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingCategoriesImagesJobStarted);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.ResizingCategoriesImagesJobAdded, (_) => ResizingCategoriesImagesJobAdded());
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationInitializationDto>(this, ServerMessages.ResizingCategoriesImagesJobStarted, (s, d) => ResizingCategoriesImagesJobStarted(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter, EntityImagesManipulationProgressDto>(this, ServerMessages.ResizingCategoriesImagesJobProgress, (s, d) => ResizingCategoriesImagesJobProgressed(d));
            MessagingCenter.Subscribe<RealTimeEventsCenter>(this, ServerMessages.WatermarkingJobEnded, (_) => ResizingCategoriesImagesJobEnded());
        }

        protected void ResizingCategoriesImagesJobAdded()
        {
            _shouldShowProgress = true;
            _status = JobStatus.Pending;
            StateHasChanged();
        }

        protected void ResizingCategoriesImagesJobEnded()
        {
            _shouldShowProgress = false;
            _status = JobStatus.Ended;
            _totalEntities = 0;
            _entitiesDone = 0;
            _entitiesLeft = 0;
            StateHasChanged();
        }

        protected void ResizingCategoriesImagesJobStarted(EntityImagesManipulationInitializationDto initializationData)
        {
            _shouldShowProgress = true;
            _status = JobStatus.Processing;
            _totalEntities = initializationData.NumberOfEntitiesToProcess;
            StateHasChanged();
        }

        protected void ResizingCategoriesImagesJobProgressed(EntityImagesManipulationProgressDto progressData)
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