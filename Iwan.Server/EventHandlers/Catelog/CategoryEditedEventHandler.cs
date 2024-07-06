using Iwan.Server.DataAccess;
using Iwan.Server.Events.Catelog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.EventHandlers.Catelog
{
    public class CategoryEditedEventHandler : INotificationHandler<CategoryEditedEvent>
    {
        protected readonly IUnitOfWork _context;

        public CategoryEditedEventHandler(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task Handle(CategoryEditedEvent notification, CancellationToken cancellationToken)
        {
            // Update the report line of unattached products
            await Task.Delay(1, cancellationToken);
        }
    }
}
