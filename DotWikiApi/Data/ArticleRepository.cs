using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotWikiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotWikiApi.Data
{
    public class ArticleRepository: IArticleRepository
    {
        private readonly DotWikiContext _context;

        public ArticleRepository(DotWikiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _context
                .Articles
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Article>> SearchArticles(string title)
        {
            return await _context
                .Articles
                .Where(article => article.Title.ToLower().Contains(title.ToLower()))
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Article> GetArticle(int id)
        {
            return _context
                .Articles
                .FirstOrDefaultAsync(article => article.Id == id);
        }

        public Task<Article> GetArticleWithSnapshots(int id)
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
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }
            await _context.Articles.AddAsync(article);
        }

        public void UpdateArticle(Article article)
        {
            // Nothing to do here
        }

        public void DeleteArticle(Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }
            _context.Articles.Remove(article);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}