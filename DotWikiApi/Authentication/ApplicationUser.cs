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
        }
        
        public virtual ICollection<Article> Articles { get; set; }
    }
}