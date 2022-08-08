using ExpenseManager.Domain.SeedWork;
using ExpenseManager.ApplicationService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ExpenseManager.Domain.Entities;
using System;
using Common.Utilities;

namespace ExpenseManager.Infrastructure.Context.EF
{
    public class ExpenseManagerDBContext : DbContext, IUnitOfWork
    {
        public ExpenseManagerDBContext(DbContextOptions<ExpenseManagerDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddPluralizingTableNameConvention();
            modelBuilder.RegisterEntityTypeConfiguration(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Expense>()
           .HasOne(i => i.Creator)
           .WithMany(u => u.CreatedExpenses)
           .HasForeignKey(i => i.CreatorId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
           .HasOne(i => i.Payer)
           .WithMany(u => u.PayededExpenses)
           .HasForeignKey(i => i.PayerId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
            .HasOne(i => i.Creator)
            .WithMany(u => u.CreatedEvents)
            .HasForeignKey(i => i.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
           .HasOne(i => i.LastUpdator)
           .WithMany(u => u.UpdatedEvents)
           .HasForeignKey(i => i.LastUpdatorId)
           .OnDelete(DeleteBehavior.Restrict);

            AddSeedData(modelBuilder);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync();
        }

        public void AddSeedData(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(
            Category.Create(1, "Food"),
            Category.Create(2, "Hotel"),
            Category.Create(3, "AirplaneTicket"));

            var passwordHash = SecurityHelper.GetSha256Hash("123456");
            modelBuilder.Entity<User>().HasData(
             User.Create(1, "MKarvar", passwordHash));

        }
    }
}

