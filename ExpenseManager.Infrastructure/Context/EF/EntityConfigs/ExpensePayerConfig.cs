using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ExpenseManager.Infrastructure.Context.EF.EntityConfigs
{
    public class ExpensePayerConfig : IEntityTypeConfiguration<ExpensePayer>
    {
        public void Configure(EntityTypeBuilder<ExpensePayer> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.ExpenseId).IsRequired();
        }
    }
}