using MediatR;

namespace Iwan.Server.Events.Vendors
{
    public record VendorAddedEvent(string VendorId) : INotification;
    public record VendorEditedEvent(string VendorId) : INotification;
    public record VendorDeletedEvent(string VendorId) : INotification;
}
