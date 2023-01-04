using System;

namespace DotWikiApi.Dtos;

public record ArticleReadDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}