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
        
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<List<Article>> SearchArticles(string title)
        {
            return await _context
                .Articles
                .Where(article => article.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }

        public async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles.FirstOrDefaultAsync(article => article.Id == id);
        }

        public async Task<Article> GetArticleWithSnapshots(int id)
        {
            return await _context
                .Articles
                .Include(article => article.Snapshots)
                .ThenInclude(snapshot => snapshot.ApplicationUser)
                .FirstOrDefaultAsync(article => article.Id == id);
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
    }
}