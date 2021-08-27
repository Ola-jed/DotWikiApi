using DotWikiApi.Authentication;
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

        public DbSet<Article> Articles { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
    }
}