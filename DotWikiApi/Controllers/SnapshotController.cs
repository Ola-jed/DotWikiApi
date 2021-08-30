using System;
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
    [Route("api/")]
    [ApiController]
    public class SnapshotController : ControllerBase
    {
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public SnapshotController(ISnapshotRepository snapshotRepository,
            IMapper mapper,
            IArticleRepository articleRepository,
            UserManager<ApplicationUser> userManager)
        {
            _snapshotRepository = snapshotRepository;
            _articleRepository = articleRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("Article/{id:int}/Snapshot")]
        public async Task<ActionResult> GetSnapshots(int id)
        {
            var articleExists = await _articleRepository.ArticleExists(id);
            if (!articleExists)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<SnapshotReadDto>>(await _snapshotRepository.GetSnapshots(id)));
        }

        [HttpGet("Snapshot/{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var snapshot = await _snapshotRepository.GetSnapshot(id);
            return snapshot == null ? NotFound() : Ok(snapshot);
        }

        [Authorize]
        [HttpDelete("Snapshot/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var snapshot = await _snapshotRepository.GetSnapshot(id);
            if (snapshot == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            if (snapshot.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }
            _snapshotRepository.DeleteSnapshot(snapshot);
            return NoContent();
        }

        [Authorize]
        [HttpPost("Snapshot/{id:int}/Rollback")]
        public async Task<ActionResult> RollbackTo(int id)
        {
            var snapshot = await _snapshotRepository.GetSnapshot(id);
            if (snapshot == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            if (snapshot.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }
            var article = await _articleRepository.GetArticle(snapshot.ArticleId);
            // Create a snapshot for this state
            var newSnapshot = new Snapshot
            {
                Title = article.Title,
                Content = article.Content,
                ArticleId = article.Id,
                ApplicationUserId = currentUser.Id,
                CreatedAt = DateTime.Now
            };
            await _snapshotRepository.CreateSnapshot(newSnapshot);
            await _snapshotRepository.SaveChanges();
            // Update the article with the selected snapshot data
            article.Content = snapshot.Content;
            _articleRepository.UpdateArticle(article);
            await _articleRepository.SaveChanges();
            return NoContent();
        }
    }
}