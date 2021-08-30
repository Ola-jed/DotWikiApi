using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Dtos
{
    public class ArticleUpdateDto
    {
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }
    }
}