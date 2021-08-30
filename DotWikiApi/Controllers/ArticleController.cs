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
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(IArticleRepository articleRepository,
            ISnapshotRepository snapshotRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _articleRepository = articleRepository;
            _snapshotRepository = snapshotRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var articles = _mapper.Map<IEnumerable<ArticleReadDto>>(await _articleRepository.GetAllArticles());
            return Ok(articles);
        }

        [HttpGet("{id:int}", Name = "Get")]
        public async Task<ActionResult<Article>> Get(int id)
        {
            var article = await _articleRepository.GetArticle(id);
            return article == null ? NotFound() : Ok(article);
        }

        [HttpGet("{id:int}/WithSnapshot")]
        public async Task<ActionResult<Article>> GetWithSnapshots(int id)
        {
            var article = await _articleRepository.GetArticleWithSnapshots(id);
            return article == null ? NotFound() : Ok(article);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArticleReadDto>> Post(ArticleCreateDto articleCreateDto)
        {
            var article = _mapper.Map<Article>(articleCreateDto);
            var usr = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            article.ApplicationUserId = usr.Id;
            article.CreatedAt = DateTime.Now;
            await _articleRepository.CreateArticle(article);
            await _articleRepository.SaveChanges();
            return CreatedAtAction(nameof(Get),new {Id = article.Id},_mapper.Map<ArticleReadDto>(article));
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ArticleUpdateDto articleUpdateDto)
        {
            var article = await _articleRepository.GetArticle(id);
            if (article == null)
            {
                return NotFound();
            }
            var usr = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            var snapshot = new Snapshot
            {
                Title = article.Title,
                Content = article.Content,
                ArticleId = article.Id,
                CreatedAt = DateTime.Now,
                ApplicationUserId = usr.Id
            };
            await _snapshotRepository.CreateSnapshot(snapshot);
            _mapper.Map(articleUpdateDto, article);
            _articleRepository.UpdateArticle(article);
            await _articleRepository.SaveChanges();
            return NoContent();
        }

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
