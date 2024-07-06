using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Hubs;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.RealTime;
using Iwan.Shared.Constants;
using Iwan.Shared.Exceptions;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Commands.Jobs
{
    public class StartProductImagesResizingJob
    {
        public record Request() : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IUnitOfWork _context;
            protected readonly IRealTimeNotifierImagesManipulatorService _realTimeNotifierImagesManipulatorService;
            protected readonly ILoggedInUserProvider _loggedInUserProvider;
            protected readonly IHubContext<AdminHub> _hubContext;

            public Handler(IUnitOfWork context, IRealTimeNotifierImagesManipulatorService realTimeNotifierImagesManipulatorService,
                ILoggedInUserProvider loggedInUserProvider, IHubContext<AdminHub> hubContext)
            {
                _context = context;
                _realTimeNotifierImagesManipulatorService = realTimeNotifierImagesManipulatorService;
                _loggedInUserProvider = loggedInUserProvider;
                _hubContext = hubContext;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var currentJob = await _context.JobDetailsRepository
                    .Where(j => (j.JobTypeId == (int)JobType.Watermarking || j.JobTypeId == (int)JobType.UnWatermarking
                                  || j.JobTypeId == (int)JobType.ResizingProductsImages) &&
                        j.JobStatusId == (int)JobStatus.Pending || j.JobStatusId == (int)JobStatus.Processing)
                    .FirstOrDefaultAsync(cancellationToken);

                if (currentJob != null)
                    throw new BadRequestException(Responses.Jobs.AlreadyRunningJobOnImages);

                var jobId = BackgroundJob.Schedule(() => _realTimeNotifierImagesManipulatorService.ResizeProductsImagesAsync(null, cancellationToken), TimeSpan.Zero);
                
                await _context.JobDetailsRepository.AddAsync(new Domain.Jobs.JobDetail
                {
                    JobId = jobId,
                    Status = JobStatus.Pending,
                    Type = JobType.ResizingProductsImages,
                }, cancellationToken);

                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

                await _hubContext.Clients.Group(SignalRGroups.ProductsImagesResizeProgress).SendAsync(ServerMessages.ResizingProductsImagesJobAdded, cancellationToken);

                return Unit.Value;
            }
        }
    }
}
