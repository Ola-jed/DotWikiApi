using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotWikiApi.Dtos;

public record ArticleCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Content is required")]
    [Column(TypeName = "text")]
    public string Content { get; set; } = null!;
}