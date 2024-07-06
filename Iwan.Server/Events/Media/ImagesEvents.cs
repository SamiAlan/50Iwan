using MediatR;

namespace Iwan.Server.Events.Media
{
    public record TempImageUploadedEvent(string TempImageId) : INotification;
    public record TempImageDeletedEvent(string TempImageId) : INotification;
}
