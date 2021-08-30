using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Dtos
{
    public class ArticleCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [Column(TypeName = "text")]
        public string Content { get; set; }
    }
}