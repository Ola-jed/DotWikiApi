using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Dtos;

public record ArticleUpdateDto
{
    [Required(ErrorMessage = "Content is required")]
    [Column(TypeName = "text")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Comment is required")]
    [MaxLength(100)]
    public string Comment { get; set; }
}