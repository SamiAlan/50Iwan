using MediatR;

namespace Iwan.Server.Events.Accounts
{
    public record AppUserAddedEvent(string AppUserId) : INotification;
    public record AppUserEditedEvent(string AppUserId) : INotification;
    public record AppUserDeletedEvent(string AppUserId) : INotification;
    public record UserChangedPasswordEvent(string AppUserId) : INotification;
    public record UserLoggedInEvent(string AppUserId) : INotification;
    public record UserRefreshedHisTokenEvent(string AppUserId) : INotification;
    public record UserDeletedEvent(string AppUserId) : INotification;
    public record UserUpdatedProfileEvent(string AppUserId) : INotification;
}
