using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ExpenseManager.Infrastructure.Context.EF.EntityConfigs
{
    public class ExpensePartTakerConfig : IEntityTypeConfiguration<ExpensePartTaker>
    {
        public void Configure(EntityTypeBuilder<ExpensePartTaker> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.PartTakerId).IsRequired();
            builder.Property(c => c.ExpenseId).IsRequired();
            builder.Property(c => c.IsPaid).HasDefaultValue(false);
        }
    }
}