using MediatR;

namespace Iwan.Server.Events.Compositions
{
    public record CompositionAddedEvent(string CompositionId) : INotification;
    public record CompositionEditedEvent(string CompositionId) : INotification;
    public record CompositionDeletedEvent(string CompositionId) : INotification;
    public record CompositionImageChangedEvent(string CompositionId) : INotification;
}
