using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Infrastructure.Context.EF.EntityConfigs
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.CreatorId).IsRequired().HasColumnName("CreatorId");
            builder.Property(c => c.CreationDateTime).IsRequired();
            builder.Property(c => c.IsFinished).HasDefaultValue(false);
        }
    }
}
