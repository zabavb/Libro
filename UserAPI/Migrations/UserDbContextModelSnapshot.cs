﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserAPI.Data;

#nullable disable

namespace UserAPI.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserAPI.Models.Password", b =>
                {
                    b.Property<Guid>("PasswordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PasswordId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Passwords");

                    b.HasData(
                        new
                        {
                            PasswordId = new Guid("fb2fa649-791f-4022-bad6-edd7bd6259c3"),
                            PasswordHash = "knBGOdI98YdYcfWvlRJy8wKzKmCwYtwuv3x1IgscOOg",
                            PasswordSalt = "xyGJrVaDX6U",
                            UserId = new Guid("6c52c0a6-24e2-4f39-bd69-96049bd9abc3")
                        },
                        new
                        {
                            PasswordId = new Guid("d52ad829-1605-44e4-9381-605567fdf3e0"),
                            PasswordHash = "ixpndSVLRN14+wm2XikLmegNeMYX2TroRYE8iW7iFdY",
                            PasswordSalt = "UucZLQtt9nI",
                            UserId = new Guid("695426ac-1a74-4d2f-bae9-f426f693ed63")
                        });
                });

            modelBuilder.Entity("UserAPI.Models.Subscription", b =>
                {
                    b.Property<Guid>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("EndDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("GETDATE() + 1");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Subscriptions");

                    b.HasData(
                        new
                        {
                            SubscriptionId = new Guid("2469c5af-92e7-45f7-ac16-f15e14d57531"),
                            Description = "",
                            EndDate = new DateTime(2026, 2, 1, 16, 6, 27, 88, DateTimeKind.Local).AddTicks(155),
                            Title = "Premium Plan",
                            UserId = new Guid("6c52c0a6-24e2-4f39-bd69-96049bd9abc3")
                        },
                        new
                        {
                            SubscriptionId = new Guid("d084f570-fbbb-4c92-8b82-12137cac52c1"),
                            Description = "",
                            EndDate = new DateTime(2027, 2, 1, 16, 6, 27, 97, DateTimeKind.Local).AddTicks(8123),
                            Title = "Premium Plan",
                            UserId = new Guid("695426ac-1a74-4d2f-bae9-f426f693ed63")
                        });
                });

            modelBuilder.Entity("UserAPI.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("PasswordId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("6c52c0a6-24e2-4f39-bd69-96049bd9abc3"),
                            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            LastName = "Doe",
                            PasswordId = new Guid("fb2fa649-791f-4022-bad6-edd7bd6259c3"),
                            PhoneNumber = "123-456-7890",
                            Role = "USER"
                        },
                        new
                        {
                            UserId = new Guid("695426ac-1a74-4d2f-bae9-f426f693ed63"),
                            DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            PasswordId = new Guid("d52ad829-1605-44e4-9381-605567fdf3e0"),
                            PhoneNumber = "987-654-3210",
                            Role = "ADMIN"
                        });
                });

            modelBuilder.Entity("UserAPI.Models.Password", b =>
                {
                    b.HasOne("UserAPI.Models.User", "User")
                        .WithOne("Password")
                        .HasForeignKey("UserAPI.Models.Password", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserAPI.Models.Subscription", b =>
                {
                    b.HasOne("UserAPI.Models.User", "User")
                        .WithOne("Subscription")
                        .HasForeignKey("UserAPI.Models.Subscription", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserAPI.Models.User", b =>
                {
                    b.Navigation("Password")
                        .IsRequired();

                    b.Navigation("Subscription");
                });
#pragma warning restore 612, 618
        }
    }
}
