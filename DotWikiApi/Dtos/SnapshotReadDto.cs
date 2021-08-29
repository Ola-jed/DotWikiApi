using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Dtos
{
    public class SnapshotReadDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public int ArticleId { get; set; }
    }
}