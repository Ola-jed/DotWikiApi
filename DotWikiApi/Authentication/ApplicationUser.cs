using System.Collections.Generic;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity;

namespace DotWikiApi.Authentication
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser() : base()
        {
            Articles = new HashSet<Article>();
            Snapshots = new HashSet<Snapshot>();
        }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; }
    }
}