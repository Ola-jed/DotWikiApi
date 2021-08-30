using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotWikiApi.Data
{
    public class DotWikiContext: IdentityDbContext<ApplicationUser>
    {
        public DotWikiContext(DbContextOptions<DotWikiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Article>()
                .HasMany(a => a.Snapshots)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>()
                .HasMany(a => a.Articles)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>()
                .HasMany(a => a.Snapshots)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
    }
}