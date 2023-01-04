namespace DotWikiApi.Dtos;

public record ArticleFilterDto(int PageSize = 10, int PageNumber = 1, string? Search = null);