using System;
using System.Collections.Generic;
using System.Text;
using Cameo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cameo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TalentCategory>()
                .HasKey(bc => new { bc.TalentId, bc.CategoryId });
            modelBuilder.Entity<TalentCategory>()
                .HasOne(bc => bc.Talent)
                .WithMany(b => b.TalentCategories)
                .HasForeignKey(bc => bc.TalentId);
            modelBuilder.Entity<TalentCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.TalentCategories)
                .HasForeignKey(bc => bc.CategoryId);


            //one to one
            modelBuilder.Entity<VideoRequest>()
                .HasOne(a => a.Invoice)
                .WithOne(b => b.VideoRequest)
                .HasForeignKey<Invoice>(b => b.VideoRequestID);
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<SocialArea> SocialAreas { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Talent> Talents { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TalentProject> TalentProjects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<VideoRequest> VideoRequests { get; set; }
        public DbSet<VideoRequestType> VideoRequestTypes { get; set; }
        public DbSet<VideoRequestStatus> VideoRequestStatuses { get; set; }
        public DbSet<LogTalentPrice> LogTalentPrice { get; set; }
        public DbSet<FirebaseRegistrationToken> FirebaseRegistrationTokens { get; set; }
        public DbSet<ClickTransaction> ClickTransactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<WithdrawRequest> WithdrawRequests { get; set; }
        public DbSet<WithdrawRequestStatus> WithdrawRequestStatuses { get; set; }
    }
}
