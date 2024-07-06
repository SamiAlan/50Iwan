using MediatR;

namespace Iwan.Server.Events.Settings
{
    public record TempImagesSettingsEditedEvent() : INotification;
    public record WatermarkSettingsEditedEvent() : INotification;
    public record WatermarkImageChangedEvent() : INotification;
}
