﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Account;

#nullable disable

namespace Mix.Database.Migrations.MySqlAccount
{
    [DbContext(typeof(MySqlAccountContext))]
    [Migration("20220423065303_RefreshTokenNotRequiredEmail")]
    partial class RefreshTokenNotRequiredEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<Guid?>("AspNetRolesId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ClaimType"), "utf8");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ClaimValue"), "utf8");

                    b.Property<Guid?>("MixRoleId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.HasKey("Id");

                    b.HasIndex("AspNetRolesId");

                    b.HasIndex("MixRoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ConcurrencyStamp"), "utf8");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("NormalizedName"), "utf8");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("(NormalizedName IS NOT NULL)");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ClaimType"), "utf8");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ClaimValue"), "utf8");

                    b.Property<Guid?>("MixUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MixUserId1")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.HasKey("Id");

                    b.HasIndex("MixUserId");

                    b.HasIndex("MixUserId1");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("LoginProvider"), "utf8");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ProviderKey"), "utf8");

                    b.Property<Guid?>("MixUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MixUserId1")
                        .HasColumnType("char(36)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ProviderDisplayName"), "utf8");

                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("PK_AspNetUserLogins_1");

                    b.HasIndex("MixUserId");

                    b.HasIndex("MixUserId1");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserRoles", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<int>("MixTenantId")
                        .HasColumnType("int");

                    b.Property<Guid?>("AspNetRolesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MixRoleId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MixUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("MixUserId1")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "RoleId", "MixTenantId");

                    b.HasIndex("AspNetRolesId");

                    b.HasIndex("MixRoleId");

                    b.HasIndex("MixUserId");

                    b.HasIndex("MixUserId1");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserTokens", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("LoginProvider"), "utf8");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8");

                    b.Property<Guid?>("MixUserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Value")
                        .HasColumnType("varchar(4000)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Value"), "utf8");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.HasIndex("MixUserId");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.Clients", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Id"), "utf8");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("AllowedOrigin")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("AllowedOrigin"), "utf8");

                    b.Property<int>("ApplicationType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8");

                    b.Property<int>("RefreshTokenLifeTime")
                        .HasColumnType("int");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Secret"), "utf8");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.MixRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ConcurrencyStamp"), "utf8");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("NormalizedName"), "utf8");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasDatabaseName("MixRoleNameIndex")
                        .HasFilter("(NormalizedName IS NOT NULL)");

                    b.ToTable("MixRoles");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.MixUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ConcurrencyStamp"), "utf8");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Email"), "utf8");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ModifiedBy"), "utf8");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("NormalizedEmail"), "utf8");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("NormalizedUserName"), "utf8");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("PasswordHash"), "utf8");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("PhoneNumber"), "utf8");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("RegisterType")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("RegisterType"), "utf8");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("SecurityStamp"), "utf8");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("UserName"), "utf8");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("(NormalizedUserName IS NOT NULL)");

                    b.ToTable("MixUsers");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.MixUserTenant", b =>
                {
                    b.Property<Guid>("MixUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.HasKey("MixUserId", "TenantId");

                    b.HasIndex("MixUserId");

                    b.HasIndex("TenantId");

                    b.ToTable("MixUserTenants");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.RefreshTokens", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValueSql("uuid()");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ClientId"), "utf8");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Email"), "utf8");

                    b.Property<DateTime>("ExpiresUtc")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("IssuedUtc")
                        .HasColumnType("datetime");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8_unicode_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Username"), "utf8");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetRoleClaims", b =>
                {
                    b.HasOne("Mix.Database.Entities.Account.AspNetRoles", null)
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("AspNetRolesId");

                    b.HasOne("Mix.Database.Entities.Account.MixRole", null)
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("MixRoleId");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserClaims", b =>
                {
                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("AspNetUserClaimsUser")
                        .HasForeignKey("MixUserId");

                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("MixUserId1");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserLogins", b =>
                {
                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("AspNetUserLoginsApplicationUser")
                        .HasForeignKey("MixUserId");

                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("AspNetUserLoginsUser")
                        .HasForeignKey("MixUserId1");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserRoles", b =>
                {
                    b.HasOne("Mix.Database.Entities.Account.AspNetRoles", null)
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("AspNetRolesId");

                    b.HasOne("Mix.Database.Entities.Account.MixRole", null)
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("MixRoleId");

                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("AspNetUserRolesApplicationUser")
                        .HasForeignKey("MixUserId");

                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("AspNetUserRolesUser")
                        .HasForeignKey("MixUserId1");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetUserTokens", b =>
                {
                    b.HasOne("Mix.Database.Entities.Account.MixUser", null)
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("MixUserId");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.AspNetRoles", b =>
                {
                    b.Navigation("AspNetRoleClaims");

                    b.Navigation("AspNetUserRoles");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.MixRole", b =>
                {
                    b.Navigation("AspNetRoleClaims");

                    b.Navigation("AspNetUserRoles");
                });

            modelBuilder.Entity("Mix.Database.Entities.Account.MixUser", b =>
                {
                    b.Navigation("AspNetUserClaimsUser");

                    b.Navigation("AspNetUserLoginsApplicationUser");

                    b.Navigation("AspNetUserLoginsUser");

                    b.Navigation("AspNetUserRolesApplicationUser");

                    b.Navigation("AspNetUserRolesUser");

                    b.Navigation("AspNetUserTokens");

                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}