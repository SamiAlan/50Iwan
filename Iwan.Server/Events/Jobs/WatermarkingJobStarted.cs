using MediatR;

namespace Iwan.Server.Events.Jobs
{
    public record WatermarkingJobStarted : INotification;
    public record UnWatermarkingJobStarted : INotification;
    public record ProductsImagesResizingJobStarted : INotification;
    public record CategoriesImagesResizingJobStarted : INotification;
    public record CompositionsImagesResizingJobStarted : INotification;
    public record SliderImagesResizingJobStarted : INotification;
    public record AboutUsImagesResizingJobStarted : INotification;
}
