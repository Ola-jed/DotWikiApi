using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Authentication;
using DotWikiApi.Data;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotWikiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ArticleController(IArticleRepository articleRepository,IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        // GET: api/Article
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var articles = _mapper.Map<IEnumerable<ArticleReadDto>>(await _articleRepository.GetAllArticles());
            return Ok(articles);
        }

        // GET: api/Article/5
        [HttpGet("{id:int}", Name = "Get")]
        public async Task<ActionResult<Article>> Get(int id)
        {
            var article = await _articleRepository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        // POST: api/Article
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArticleReadDto>> Post(ArticleCreateDto articleCreateDto)
        {
            var article = _mapper.Map<Article>(articleCreateDto);
            var usr = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            article.ApplicationUser = usr;
            article.CreatedAt = DateTime.Now;
            await _articleRepository.CreateArticle(article);
            await _articleRepository.SaveChanges();
            return CreatedAtAction(nameof(Get),new {Id = article.Id},_mapper.Map<ArticleReadDto>(article));
        }

        // PUT: api/Article/5
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ArticleUpdateDto articleUpdateDto)
        {
            var article = await _articleRepository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            // Create the snapshot corresponding to the current state
            return NoContent();
        }

        // DELETE: api/Article/5
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var article = await _articleRepository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            if (article.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }
            _articleRepository.DeleteArticle(article);
            await _articleRepository.SaveChanges();
            return NoContent();
        }
    }
}
