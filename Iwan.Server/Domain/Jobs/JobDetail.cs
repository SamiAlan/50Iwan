using Iwan.Shared.Constants;

namespace Iwan.Server.Domain.Jobs
{
    public class JobDetail : BaseEntity
    {
        public int JobStatusId { get; set; }
        public int JobTypeId { get; set; }
        public string JobId { get; set; }

        public JobType Type
        {
            get => (JobType)JobTypeId;
            set => JobTypeId = (int)value;
        }

        public JobStatus Status
        {
            get => (JobStatus)JobStatusId;
            set => JobStatusId = (int)value;
        }
    }
}
