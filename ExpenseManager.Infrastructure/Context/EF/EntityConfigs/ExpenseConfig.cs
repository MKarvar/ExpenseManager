using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Infrastructure.Context.EF.EntityConfigs
{
    public class ExpenseConfig : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.CreatorId).IsRequired().HasColumnName("CreatorId");
            builder.Property(c => c.PayerId).IsRequired().HasColumnName("PayerId");
            builder.Property(c => c.CategoryId).IsRequired();
            builder.Property(c => c.EventId).IsRequired();
            builder.Property(c => c.CreationDateTime).IsRequired();
            builder.Property(c => c.PayDateTime).IsRequired();
            builder.Property(c => c.TotalPrice).IsRequired().HasDefaultValue(0);
        }
    }
}
