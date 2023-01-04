using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DotWikiApi.Models;

public class ApplicationUser: IdentityUser
{
    [Required]
    public DateTime RegisterDate { get; set; }
    public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    public virtual ICollection<Snapshot> Snapshots { get; set; } = new HashSet<Snapshot>();
}