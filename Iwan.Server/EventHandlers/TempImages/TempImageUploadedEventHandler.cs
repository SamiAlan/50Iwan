using Iwan.Server.DataAccess;
using Iwan.Server.Events.Media;
using Iwan.Server.Hubs;
using Iwan.Server.Infrastructure.Media;
using Iwan.Server.Services.Media;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.EventHandlers.TempImages
{
    public class TempImageUploadedEventHandler : INotificationHandler<TempImageUploadedEvent>
    {
        protected readonly IHubContext<AdminHub> _hubContext;
        protected readonly IUnitOfWork _context;
        protected readonly IAppImageHelper _appImageHelper;

        public TempImageUploadedEventHandler(IHubContext<AdminHub> hubContext,
            IUnitOfWork context, IAppImageHelper appImageHelper)
        {
            _hubContext = hubContext;
            _context = context;
            _appImageHelper = appImageHelper;
        }

        public async Task Handle(TempImageUploadedEvent eventData, CancellationToken cancellationToken)
        {
            var image = await _context.TempImagesRepository.FindAsync(eventData.TempImageId, cancellationToken);

            var dto = _appImageHelper.GenerateImageDto(image);

            await _hubContext.Clients.Group("TempImageUploaded")
                .SendAsync("TempImageUploaded", dto.ToJson(), cancellationToken);
        }
    }
}
