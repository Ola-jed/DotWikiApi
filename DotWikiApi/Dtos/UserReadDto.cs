using System;
using System.Collections.Generic;
using DotWikiApi.Models;

namespace DotWikiApi.Dtos;

public record UserReadDto
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime RegisterDate { get; set; }

    public ICollection<Article> Articles { get; set; }
}