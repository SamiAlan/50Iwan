using MediatR;

namespace Iwan.Server.Events.Common
{
    public record AddressAddedEvent(string AddressId) : INotification;
    public record AddressEditedEvent(string AddressId) : INotification;
    public record AddressDeletedEvent(string AddressId) : INotification;
}
