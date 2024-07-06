using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Shared.Constants;
using Iwan.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Iwan.Server.Services.RealTime;
using System;
using Iwan.Server.Services.Accounts;
using Microsoft.AspNetCore.SignalR;
using Iwan.Server.Hubs;

namespace Iwan.Server.Commands.Jobs
{
    public class StartUnWatermarkingJob
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
                // Basically get a job that is related to products 
                var currentJob = await _context.JobDetailsRepository
                    .Where(j => (j.JobTypeId == (int)JobType.Watermarking || j.JobTypeId == (int)JobType.UnWatermarking 
                                 || j.JobTypeId == (int) JobType.ResizingProductsImages) &&
                        (j.JobStatusId == (int)JobStatus.Pending || j.JobStatusId == (int)JobStatus.Processing))
                    .FirstOrDefaultAsync(cancellationToken);

                if (currentJob != null)
                    throw new BadRequestException(Responses.Jobs.AlreadyRunningJobOnImages);

                var jobId = BackgroundJob.Schedule(() => _realTimeNotifierImagesManipulatorService.RemoveWatermarkFromImagesAsync(null, cancellationToken), TimeSpan.FromSeconds(10));
                await _context.JobDetailsRepository.AddAsync(new Domain.Jobs.JobDetail
                {
                    JobId = jobId,
                    Status = JobStatus.Pending,
                    Type = JobType.UnWatermarking,
                }, cancellationToken);

                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

                await _hubContext.Clients.Group(SignalRGroups.UnWatermarkProgress).SendAsync(ServerMessages.UnWatermarkingJobAdded, cancellationToken);

                ServerState.WorkingOnProducts = true;

                return Unit.Value;
            }
        }
    }
}
