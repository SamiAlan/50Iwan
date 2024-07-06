using Iwan.Server.DataAccess;
using Iwan.Server.Events.Catelog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.EventHandlers.Catelog
{
    public class CategoryDeletedEventHandler : INotificationHandler<CategoryDeletedEvent>
    {
        protected readonly IUnitOfWork _context;

        public CategoryDeletedEventHandler(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task Handle(CategoryDeletedEvent notification, CancellationToken cancellationToken)
        {
            // Update the report line of unattached products
            await Task.Delay(1, cancellationToken);
        }
    }
}
