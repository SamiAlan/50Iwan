using Iwan.Server.Domain.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Jobs
{
    public class JobDetailBuilder : EntityBuilder<JobDetail>
    {
        public override void Configure(EntityTypeBuilder<JobDetail> builder)
        {
            builder.ToTable("JobDetails");
            builder.HasKey(j => j.Id);

            // Ignores
            builder.Ignore(j => j.Type);
            builder.Ignore(j => j.Status);
        }
    }
}
