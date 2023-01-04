using System.Linq;
using System.Threading.Tasks;
using DotWikiApi.Data.Contracts;
using DotWikiApi.Models;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;

namespace DotWikiApi.Data;

public class ArticleRepository: IArticleRepository
{
    private readonly DotWikiContext _context;

    public ArticleRepository(DotWikiContext context)
    {
        _context = context;
    }

    public async Task<Page<Article>> GetArticles(PaginationParameter paginationParameter, string? search = null)
    {
        var query = _context.Articles.AsNoTracking();
        if (search != null)
        {
            query = query.Where(x => EF.Functions.Like(x.Title, $"%{search}%"));
        }

        return await Task.Run(() => query.Paginate(paginationParameter, x => x.CreatedAt));
    }

    public Task<Article?> GetArticle(int id)
    {
        return _context
            .Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(article => article.Id == id);
    }

    public Task<Article?> GetArticleWithSnapshots(int id)
    {
        return _context
            .Articles
            .Include(a => a.Snapshots)
            .FirstOrDefaultAsync(article => article.Id == id);
    }

    public Task<bool> ArticleExists(int id)
    {
        return _context.Articles.AnyAsync(a => a.Id == id);
    }

    public async Task CreateArticle(Article article)
    {
        await _context.Articles.AddAsync(article);
    }

    public void UpdateArticle(Article _)
    {
        // Nothing to do here
    }

    public void DeleteArticle(Article article)
    {
        _context.Articles.Remove(article);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}