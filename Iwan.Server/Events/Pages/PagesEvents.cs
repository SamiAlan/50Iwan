using MediatR;

namespace Iwan.Server.Events.Pages
{
    public record HeaderSectionEditedEvent : INotification;
    public record ContactUsSectionEditedEvent : INotification;
    public record AboutUsSectionEditedEvent : INotification;
    public record ServicesSectionEditedEvent : INotification;
    public record InteriorDesignSectionEditedEvent : INotification;
    public record InteriorDesignSectionImageChangedEvent : INotification;
    public record InteriorDesignSectionMobileImageChangedEvent : INotification;
    public record AboutUsSectionImageAddedEvent(string ImageId) : INotification;
    public record AboutUsImageDeletedEvent(string ImageId) : INotification;
    public record ColorAddedEvent(string ColorId) : INotification;
    public record ColorDeletedEvent(string ColorId) : INotification;
}
