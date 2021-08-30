using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity;

namespace DotWikiApi.Authentication
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser()
        {
            Articles = new HashSet<Article>();
            Snapshots = new HashSet<Snapshot>();
        }

        [Required]
        public DateTime RegisterDate { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; }
    }
}