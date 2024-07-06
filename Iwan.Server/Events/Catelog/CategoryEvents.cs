using MediatR;

namespace Iwan.Server.Events.Catelog
{
    public record CategoryAddedEvent(string CategoryId) : INotification;
    public record CategoryDeletedEvent(string CategoryId) : INotification;
    public record CategoryEditedEvent(string CategoryId) : INotification;
    public record CategoryImageChangedEvent(string CategoryId) : INotification;
    public record CategoryImageEditedEvent(string CategoryId) : INotification;
}
