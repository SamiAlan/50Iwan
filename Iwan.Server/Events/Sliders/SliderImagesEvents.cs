using MediatR;

namespace Iwan.Server.Events.Sliders
{
    public record SliderImageAddedEvent(string SliderImageId) : INotification;
    public record SliderImageEditedEvent(string SliderImageId) : INotification;
    public record SliderImageDeletedEvent(string SliderImageId) : INotification;

}
