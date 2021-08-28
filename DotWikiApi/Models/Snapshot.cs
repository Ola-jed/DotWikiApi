using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotWikiApi.Authentication;

namespace DotWikiApi.Models
{
    public class Snapshot
    {
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
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}