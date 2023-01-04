using System;

namespace DotWikiApi.Dtos;

public record SnapshotReadDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    
    public int ArticleId { get; set; }
}