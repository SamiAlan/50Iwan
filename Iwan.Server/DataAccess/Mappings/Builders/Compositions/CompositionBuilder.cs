using Iwan.Server.Domain.Compositions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Compositions
{
    public class CompositionBuilder : EntityBuilder<Composition>
    {
        public override void Configure(EntityTypeBuilder<Composition> builder)
        {
            // Table name
            builder.ToTable("Compositions");

            // Key
            builder.HasKey(p => p.Id);

            // Ignore color type
            builder.Ignore(c => c.ColorType);
        }
    }
}
