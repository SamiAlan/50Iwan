using Iwan.Server.Domain.Catelog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iwan.Server.DataAccess.Mappings.Builders.Catelog
{
    public class CategoryBuilder : EntityBuilder<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table name
            builder.ToTable("Categories");

            // Key
            builder.HasKey(c => c.Id);

            // Relations
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Color type
            builder.Ignore(p => p.ColorType);
        }
    }
}
