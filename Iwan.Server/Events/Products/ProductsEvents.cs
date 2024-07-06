using MediatR;

namespace Iwan.Server.Events.Products
{
    public record ProductAddedEvent(string ProductId) : INotification;
    public record ProductEditedEvent(string ProductId) : INotification;
    public record ProductDeletedEvent(string ProductId) : INotification;
    public record ProductImageDeletedEvent(string ProductImageId) : INotification;
    public record ProductImageAddedEvent(string ProductImageId) : INotification;
    public record ProductAddedToCategoryEvent(string ProductCategoryId) : INotification;
    public record ProductStateAddedEvent(string StateId) : INotification;
    public record ProductStateDeletedEvent(string StateId) : INotification;
    public record ProductRemovedFromCategoryEvent(string ProductId, string CategoryId) : INotification;
    public record ProductMainImageSetEvent(string ProductImageId) : INotification;
}
