using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Dtos
{
    public record ArticleReadDto
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
    }
}