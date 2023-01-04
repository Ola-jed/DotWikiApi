using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Data.Contracts;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using DotWikiApi.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotWikiApi.Controllers;

[Route("api/")]
[ApiController]
public class SnapshotsController : ControllerBase
{
    private readonly ISnapshotRepository _snapshotRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;
    private readonly IApplicationUserService _userService;

    public SnapshotsController(ISnapshotRepository snapshotRepository,
        IMapper mapper,
        IArticleRepository articleRepository,
        IApplicationUserService userService)
    {
        _snapshotRepository = snapshotRepository;
        _articleRepository = articleRepository;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("Article/{id:int}/Snapshot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SnapshotReadDto>>> GetSnapshots(int id)
    {
        var articleExists = await _articleRepository.ArticleExists(id);
        if (!articleExists)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<IEnumerable<SnapshotReadDto>>(await _snapshotRepository.GetSnapshots(id)));
    }

    [HttpGet("Snapshot/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Snapshot>> Get(int id)
    {
        var snapshot = await _snapshotRepository.GetSnapshot(id);
        return snapshot == null ? NotFound() : Ok(snapshot);
    }

    [Authorize]
    [HttpDelete("Snapshot/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var snapshot = await _snapshotRepository.GetSnapshot(id);
        if (snapshot == null)
        {
            return NotFound();
        }
        
        var currentUser = (await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!))!;
        if (snapshot.ApplicationUserId != currentUser.Id)
        {
            return Forbid();
        }
        
        _snapshotRepository.DeleteSnapshot(snapshot);
        await _snapshotRepository.SaveChanges();
        return NoContent();
    }

    [Authorize]
    [HttpPost("Snapshot/{id:int}/Rollback")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RollbackTo(int id)
    {
        var snapshot = await _snapshotRepository.GetSnapshot(id);
        if (snapshot == null)
        {
            return NotFound();
        }
        
        var currentUser = (await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!))!;
        if (snapshot.ApplicationUserId != currentUser.Id)
        {
            return Forbid();
        }
        var article = (await _articleRepository.GetArticle(snapshot.ArticleId))!;
        
        await _snapshotRepository.CreateSnapshot(new Snapshot
        {
            Title = article.Title,
            Content = article.Content,
            ArticleId = article.Id,
            Comment = "Rollback",
            ApplicationUserId = currentUser.Id,
            CreatedAt = DateTime.Now
        });
        await _snapshotRepository.SaveChanges();
        article.Content = snapshot.Content;
        _articleRepository.UpdateArticle(article);
        await _articleRepository.SaveChanges();
        return NoContent();
    }
}