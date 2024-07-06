using MediatR;

namespace Iwan.Server.Events.Settings
{
    public record CategoriesImagesSettingsEditedEvent() : INotification;
    public record ProductsImagesSettingsEditedEvent() : INotification;
    public record CompositionsImagesSettingsEditedEvent(): INotification;
    public record AboutUsSectionImagesSettingsEditedEvent() : INotification;
    public record SlidersImagesSettingsEditedEvent() : INotification;
}
