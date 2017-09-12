using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChatItUp.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatItUp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Thread>()
               .Property(b => b.created)
               .HasDefaultValueSql("GETDATE()");
            builder.Entity<ThreadPost>()
               .Property(b => b.dateCreatd)
               .HasDefaultValueSql("GETDATE()");
            builder.Entity<Relation>()
               .Property(b => b.ConnectedOn)
               .HasDefaultValueSql("GETDATE()");
        }

        public DbSet<ChatItUp.Models.Category> Category { get; set; }

        public DbSet<ChatItUp.Models.Forum> Forum { get; set; }

        public DbSet<ChatItUp.Models.Thread> Thread { get; set; }

        public DbSet<ChatItUp.Models.ThreadPost> ThreadPost { get; set; }

        public DbSet<ChatItUp.Models.ApplicationUser> ApplicationUser { get; set; }

        public DbSet<ChatItUp.Models.Relation> Relation { get; set; }
    }
}
