﻿// <auto-generated />
using System;
using ExpenseManager.Infrastructure.Context.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExpenseManager.Infrastructure.Migrations
{
    [DbContext(typeof(ExpenseManagerDBContext))]
    [Migration("20220726211926_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Food"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Hotel"
                        },
                        new
                        {
                            Id = 3,
                            Name = "AirplaneTicket"
                        });
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int")
                        .HasColumnName("CreatorId");

                    b.Property<bool>("IsFinished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastUpdatorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("LastUpdatorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.EventParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("EventParticipants");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int")
                        .HasColumnName("CreatorId");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PayDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PayerId")
                        .HasColumnType("int")
                        .HasColumnName("PayerId");

                    b.Property<long>("TotalPrice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("EventId");

                    b.HasIndex("PayerId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.ExpenseTakeParter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPaid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("SettleDateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("ShareAmount")
                        .HasColumnType("float");

                    b.Property<int>("TakeParterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("TakeParterId");

                    b.ToTable("ExpenseTakeParters");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SecurityStamp")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            PasswordHash = "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=",
                            RegistrationDateTime = new DateTime(2022, 7, 27, 1, 49, 26, 40, DateTimeKind.Local).AddTicks(827),
                            SecurityStamp = new Guid("d83ea151-9d27-4803-a7e9-9b6ae9f29138"),
                            Username = "MKarvar"
                        });
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Category", b =>
                {
                    b.HasOne("ExpenseManager.Domain.Entities.Category", "ParentCategory")
                        .WithMany("ChildCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Event", b =>
                {
                    b.HasOne("ExpenseManager.Domain.Entities.User", "Creator")
                        .WithMany("CreatedEvents")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpenseManager.Domain.Entities.User", "LastUpdator")
                        .WithMany("UpdatedEvents")
                        .HasForeignKey("LastUpdatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Creator");

                    b.Navigation("LastUpdator");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.EventParticipant", b =>
                {
                    b.HasOne("ExpenseManager.Domain.Entities.Event", "Event")
                        .WithMany("EventParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpenseManager.Domain.Entities.User", "Participant")
                        .WithMany("EventParticipants")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Expense", b =>
                {
                    b.HasOne("ExpenseManager.Domain.Entities.Category", "Category")
                        .WithMany("Expenses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpenseManager.Domain.Entities.User", "Creator")
                        .WithMany("CreatedExpenses")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpenseManager.Domain.Entities.Event", "Event")
                        .WithMany("EventExpenses")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpenseManager.Domain.Entities.User", "Payer")
                        .WithMany("PayededExpenses")
                        .HasForeignKey("PayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Creator");

                    b.Navigation("Event");

                    b.Navigation("Payer");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.ExpenseTakeParter", b =>
                {
                    b.HasOne("ExpenseManager.Domain.Entities.Expense", "Expense")
                        .WithMany("ExpenseParticipants")
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpenseManager.Domain.Entities.User", "TakeParter")
                        .WithMany("ExpenseParticipants")
                        .HasForeignKey("TakeParterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Expense");

                    b.Navigation("TakeParter");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Category", b =>
                {
                    b.Navigation("ChildCategories");

                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Event", b =>
                {
                    b.Navigation("EventExpenses");

                    b.Navigation("EventParticipants");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.Expense", b =>
                {
                    b.Navigation("ExpenseParticipants");
                });

            modelBuilder.Entity("ExpenseManager.Domain.Entities.User", b =>
                {
                    b.Navigation("CreatedEvents");

                    b.Navigation("CreatedExpenses");

                    b.Navigation("EventParticipants");

                    b.Navigation("ExpenseParticipants");

                    b.Navigation("PayededExpenses");

                    b.Navigation("UpdatedEvents");
                });
#pragma warning restore 612, 618
        }
    }
}