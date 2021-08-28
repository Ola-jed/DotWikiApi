using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotWikiApi.Models;

namespace DotWikiApi.Data
{
    public interface IArticleRepository
    {
        Task<bool> SaveChanges();
        Task<IEnumerable<Article>> GetAllArticles();
        Task<List<Article>> SearchArticles(string title);
        Task<Article> GetArticle(int id);
        Task CreateArticle(Article article);
        void UpdateArticle(Article article);
        void DeleteArticle(Article article);
    }
}