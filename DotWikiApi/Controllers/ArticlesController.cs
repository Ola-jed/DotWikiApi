using System;
using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Data.Contracts;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using DotWikiApi.Services.User;
using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotWikiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;
    private readonly ISnapshotRepository _snapshotRepository;
    private readonly IMapper _mapper;
    private readonly IApplicationUserService _userService;

    public ArticlesController(IArticleRepository articleRepository,
        ISnapshotRepository snapshotRepository,
        IMapper mapper,
        IApplicationUserService userService)
    {
        _articleRepository = articleRepository;
        _snapshotRepository = snapshotRepository;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<Page<ArticleReadDto>> GetAll([FromQuery] ArticleFilterDto articleFilterDto)
    {
        var paginationParameter = new PaginationParameter(articleFilterDto.PageSize, articleFilterDto.PageNumber);
        var articles = await _articleRepository.GetArticles(paginationParameter, articleFilterDto.Search);
        return articles.Map(x => _mapper.Map<ArticleReadDto>(x));
    }

    [HttpGet("{id:int}", Name = "Get")]
    public async Task<ActionResult<Article>> Get(int id)
    {
        var article = await _articleRepository.GetArticle(id);
        return article == null ? NotFound() : article;
    }

    [HttpGet("{id:int}/WithSnapshot")]
    public async Task<ActionResult<Article>> GetWithSnapshots(int id)
    {
        var article = await _articleRepository.GetArticleWithSnapshots(id);
        return article == null ? NotFound() : article;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post(ArticleCreateDto articleCreateDto)
    {
        var article = _mapper.Map<Article>(articleCreateDto);
        var usr = (await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!))!;
        article.ApplicationUserId = usr.Id;
        article.CreatedAt = DateTime.Now;
        await _articleRepository.CreateArticle(article);
        await _articleRepository.SaveChanges();
        return CreatedAtAction(nameof(Get), new { article.Id }, _mapper.Map<ArticleReadDto>(article));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(int id, ArticleUpdateDto articleUpdateDto)
    {
        var article = await _articleRepository.GetArticle(id);
        if (article == null)
        {
            return NotFound();
        }

        var usr = (await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!))!;
        await _snapshotRepository.CreateSnapshot(new Snapshot
        {
            Title = article.Title,
            Content = article.Content,
            ArticleId = article.Id,
            Comment = articleUpdateDto.Comment,
            CreatedAt = DateTime.Now,
            ApplicationUserId = usr.Id
        });
        await _snapshotRepository.SaveChanges();
        _mapper.Map(articleUpdateDto, article);
        _articleRepository.UpdateArticle(article);
        await _articleRepository.SaveChanges();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var article = await _articleRepository.GetArticle(id);
        if (article == null)
        {
            return NotFound();
        }

        var currentUser = (await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!))!;
        if (article.ApplicationUserId != currentUser.Id)
        {
            return Forbid();
        }

        _articleRepository.DeleteArticle(article);
        await _articleRepository.SaveChanges();
        return NoContent();
    }
}