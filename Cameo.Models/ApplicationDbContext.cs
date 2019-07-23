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

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().HasMany(s => s.Students).WithOne(s => s.School);
        }*/

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<SocialArea> SocialAreas { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Talent> Talents { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
    }
}
