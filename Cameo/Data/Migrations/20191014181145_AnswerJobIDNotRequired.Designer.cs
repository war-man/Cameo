﻿// <auto-generated />
using System;
using Cameo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cameo.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191014181145_AnswerJobIDNotRequired")]
    partial class AnswerJobIDNotRequired
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cameo.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("CustomTag");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

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

                    b.Property<string>("UserType");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Cameo.Models.Attachment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("GUID")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("MimeType")
                        .HasMaxLength(128);

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<long>("Size");

                    b.HasKey("ID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Cameo.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Cameo.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AvatarID");

                    b.Property<string>("Bio");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("FirstName")
                        .HasMaxLength(256);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .HasMaxLength(256);

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("SocialAreaHandle");

                    b.Property<int?>("SocialAreaID");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("AvatarID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("SocialAreaID");

                    b.HasIndex("UserID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Cameo.Models.Post", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorID");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Cameo.Models.SocialArea", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("SocialAreas");
                });

            modelBuilder.Entity("Cameo.Models.Talent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AvatarID");

                    b.Property<string>("Bio");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("CreditCardExpire")
                        .HasMaxLength(5);

                    b.Property<string>("CreditCardNumber")
                        .HasMaxLength(32);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("FirstName")
                        .HasMaxLength(256);

                    b.Property<string>("FollowersCount");

                    b.Property<bool>("IsAvailable");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .HasMaxLength(256);

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("Price");

                    b.Property<int>("ResponseTime");

                    b.Property<string>("SocialAreaHandle");

                    b.Property<int?>("SocialAreaID");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("AvatarID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("SocialAreaID");

                    b.HasIndex("UserID");

                    b.ToTable("Talents");
                });

            modelBuilder.Entity("Cameo.Models.TalentCategory", b =>
                {
                    b.Property<int>("TalentId");

                    b.Property<int>("CategoryId");

                    b.HasKey("TalentId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("TalentCategory");
                });

            modelBuilder.Entity("Cameo.Models.TalentProject", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<int>("TalentID");

                    b.HasKey("ID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("TalentID");

                    b.ToTable("TalentProjects");
                });

            modelBuilder.Entity("Cameo.Models.VideoRequest", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AnswerDeadline");

                    b.Property<string>("AnswerJobID");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CustomerID");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("From");

                    b.Property<string>("Instructions")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsNotPublic");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("Price");

                    b.Property<int>("TalentID");

                    b.Property<string>("To")
                        .IsRequired();

                    b.Property<int>("TypeID");

                    b.HasKey("ID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("TalentID");

                    b.HasIndex("TypeID");

                    b.ToTable("VideoRequests");
                });

            modelBuilder.Entity("Cameo.Models.VideoRequestType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("VideoRequestTypes");
                });

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("Cameo.Models.Attachment", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");
                });

            modelBuilder.Entity("Cameo.Models.Category", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");
                });

            modelBuilder.Entity("Cameo.Models.Customer", b =>
                {
                    b.HasOne("Cameo.Models.Attachment", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarID");

                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("Cameo.Models.SocialArea", "SocialArea")
                        .WithMany()
                        .HasForeignKey("SocialAreaID");

                    b.HasOne("Cameo.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Cameo.Models.Post", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Author")
                        .WithMany("PostsAuthored")
                        .HasForeignKey("AuthorID");

                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany("PostsCreated")
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany("PostsModified")
                        .HasForeignKey("ModifiedBy");
                });

            modelBuilder.Entity("Cameo.Models.SocialArea", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");
                });

            modelBuilder.Entity("Cameo.Models.Talent", b =>
                {
                    b.HasOne("Cameo.Models.Attachment", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarID");

                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("Cameo.Models.SocialArea", "SocialArea")
                        .WithMany()
                        .HasForeignKey("SocialAreaID");

                    b.HasOne("Cameo.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Cameo.Models.TalentCategory", b =>
                {
                    b.HasOne("Cameo.Models.Category", "Category")
                        .WithMany("TalentCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cameo.Models.Talent", "Talent")
                        .WithMany("TalentCategories")
                        .HasForeignKey("TalentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cameo.Models.TalentProject", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("Cameo.Models.Talent", "Talent")
                        .WithMany("Projects")
                        .HasForeignKey("TalentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cameo.Models.VideoRequest", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("Cameo.Models.Talent", "Talent")
                        .WithMany()
                        .HasForeignKey("TalentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Cameo.Models.VideoRequestType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Cameo.Models.VideoRequestType", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("Cameo.Models.ApplicationUser", "Modifier")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");
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
                    b.HasOne("Cameo.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser")
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

                    b.HasOne("Cameo.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Cameo.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
