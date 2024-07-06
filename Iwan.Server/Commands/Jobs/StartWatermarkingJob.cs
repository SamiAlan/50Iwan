using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Services.Media;
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
using Iwan.Server.Services.Settings;
using Iwan.Shared.Extensions;
using Iwan.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Iwan.Server.Commands.Jobs
{
    public class StartWatermarkingJob
    {
        public record Request() : IRequest<Unit>;

        public class Handler : IRequestHandler<Request, Unit>
        {
            protected readonly IUnitOfWork _context;
            protected readonly IRealTimeNotifierImagesManipulatorService _realTimeNotifierImagesManipulatorService;
            protected readonly ILoggedInUserProvider _loggedInUserProvider;
            protected readonly IQuerySettingService _querySettingService;
            protected readonly IHubContext<AdminHub> _hubContext;

            public Handler(IUnitOfWork context, IRealTimeNotifierImagesManipulatorService realTimeNotifierImagesManipulatorService,
                ILoggedInUserProvider loggedInUserProvider, IQuerySettingService querySettingService, IHubContext<AdminHub> hubContext)
            {
                _context = context;
                _realTimeNotifierImagesManipulatorService = realTimeNotifierImagesManipulatorService;
                _loggedInUserProvider = loggedInUserProvider;
                _querySettingService = querySettingService;
                _hubContext = hubContext;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var watermarkSettings = await _querySettingService.GetWatermarkImageSettingsAsync(cancellationToken);

                if (watermarkSettings.WatermarkImageId.IsNullOrEmptyOrWhiteSpaceSafe())
                    throw new BadRequestException(Responses.Settings.CantWatermarkWithoutImage);

                var currentJob = await _context.JobDetailsRepository
                    .Where(j => ( j.JobTypeId == (int)JobType.Watermarking || j.JobTypeId == (int)JobType.UnWatermarking
                                  || j.JobTypeId == (int)JobType.ResizingProductsImages) && 
                        j.JobStatusId == (int)JobStatus.Pending || j.JobStatusId == (int)JobStatus.Processing)
                    .FirstOrDefaultAsync(cancellationToken);

                if (currentJob != null)
                    throw new BadRequestException(Responses.Jobs.AlreadyRunningJobOnImages);

                var jobId = BackgroundJob.Schedule(() => _realTimeNotifierImagesManipulatorService.AddWatermarkToImagesAsync(null, cancellationToken), TimeSpan.FromSeconds(10));
                await _context.JobDetailsRepository.AddAsync(new Domain.Jobs.JobDetail
                {
                    JobId = jobId,
                    Status = JobStatus.Pending,
                    Type = JobType.Watermarking,
                }, cancellationToken);

                await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

                await _hubContext.Clients.Group(SignalRGroups.WatermarkProgress).SendAsync(ServerMessages.WatermarkingJobAdded, cancellationToken);

                ServerState.WorkingOnProducts = true;

                return Unit.Value;
            }
        }
    }
}
