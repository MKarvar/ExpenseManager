using ExpenseManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Infrastructure.Context.EF.EntityConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Username).IsRequired();
            builder.Property(c => c.PasswordHash).IsRequired();
            builder.Property(c => c.RegistrationDateTime).IsRequired();
            builder.Property(c => c.SecurityStamp).IsRequired();
        }
    }
}
