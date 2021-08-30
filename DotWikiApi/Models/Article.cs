using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Models
{
    public class Article
    {
        public Article()
        {
            Snapshots = new HashSet<Snapshot>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; }
    }
}