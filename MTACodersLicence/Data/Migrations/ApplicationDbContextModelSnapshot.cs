﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using MTACodersLicence.Data;
using System;

namespace WebApplication5.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ChallengeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Desciption");

                    b.Property<string>("Hint");

                    b.Property<string>("Name");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("Tasks");

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Challenges");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ChallengeModels.BatteryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChallengeId");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ChallengeId");

                    b.ToTable("Batteries");
                });

            modelBuilder.Entity("MTACodersLicence.Models.GroupItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddeDateTime");

                    b.Property<string>("EMail");

                    b.Property<string>("FirstName");

                    b.Property<int>("GroupId");

                    b.Property<string>("LastName");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupItems");
                });

            modelBuilder.Entity("MTACodersLicence.Models.GroupModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ResultModels.ResultModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BatteryId");

                    b.Property<int>("SolutionId");

                    b.HasKey("Id");

                    b.HasIndex("BatteryId");

                    b.HasIndex("SolutionId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ResultModels.TestResultModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("PointsGiven");

                    b.Property<int>("ResultId");

                    b.Property<string>("ResultedOutput");

                    b.Property<int>("TestId");

                    b.HasKey("Id");

                    b.HasIndex("ResultId");

                    b.HasIndex("TestId");

                    b.ToTable("TestResults");
                });

            modelBuilder.Entity("MTACodersLicence.Models.SolutionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("ChallengeId");

                    b.Property<string>("Code");

                    b.Property<DateTime>("Duration");

                    b.Property<float>("Grade");

                    b.Property<string>("Language");

                    b.Property<DateTime>("ReceiveDateTime");

                    b.Property<float>("Score");

                    b.Property<bool>("Verified");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ChallengeId");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("MTACodersLicence.Models.TestModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BatteryId");

                    b.Property<string>("ExpectedOutput");

                    b.Property<string>("Input");

                    b.Property<float>("Points");

                    b.HasKey("Id");

                    b.HasIndex("BatteryId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MTACodersLicence.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MTACodersLicence.Models.ChallengeModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Challenges")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ChallengeModels.BatteryModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ChallengeModel", "Challenge")
                        .WithMany("Batteries")
                        .HasForeignKey("ChallengeId");
                });

            modelBuilder.Entity("MTACodersLicence.Models.GroupItem", b =>
                {
                    b.HasOne("MTACodersLicence.Models.GroupModel", "Group")
                        .WithMany("Items")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MTACodersLicence.Models.GroupModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Groups")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("MTACodersLicence.Models.ResultModels.ResultModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ChallengeModels.BatteryModel", "Battery")
                        .WithMany()
                        .HasForeignKey("BatteryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MTACodersLicence.Models.SolutionModel", "Solution")
                        .WithMany("Results")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MTACodersLicence.Models.ResultModels.TestResultModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ResultModels.ResultModel", "Result")
                        .WithMany("TestResults")
                        .HasForeignKey("ResultId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MTACodersLicence.Models.TestModel", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MTACodersLicence.Models.SolutionModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("MTACodersLicence.Models.ChallengeModel", "Challenge")
                        .WithMany("Solutions")
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MTACodersLicence.Models.TestModel", b =>
                {
                    b.HasOne("MTACodersLicence.Models.ChallengeModels.BatteryModel", "Battery")
                        .WithMany("Tests")
                        .HasForeignKey("BatteryId");
                });
#pragma warning restore 612, 618
        }
    }
}
