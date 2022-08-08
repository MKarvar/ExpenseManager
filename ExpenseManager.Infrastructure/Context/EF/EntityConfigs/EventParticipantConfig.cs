using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ExpenseManager.Infrastructure.Context.EF.EntityConfigs
{
    public class EventParticipantConfig : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.ParticipantId).IsRequired();
            builder.Property(c => c.EventId).IsRequired();
        }
    }
}