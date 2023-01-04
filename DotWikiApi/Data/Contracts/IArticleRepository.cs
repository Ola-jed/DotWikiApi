using System.Collections.Generic;
using System.Threading.Tasks;
using DotWikiApi.Models;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace DotWikiApi.Data.Contracts;

public interface IArticleRepository
{
    Task SaveChanges();
    Task<Page<Article>> GetArticles(PaginationParameter paginationParameter, string? search = null);
    Task<Article?> GetArticle(int id);
    Task<Article?> GetArticleWithSnapshots(int id);
    Task<bool> ArticleExists(int id);
    Task CreateArticle(Article article);
    void UpdateArticle(Article _);
    void DeleteArticle(Article article);
}