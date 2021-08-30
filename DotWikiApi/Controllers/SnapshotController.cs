using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Authentication;
using DotWikiApi.Data;
using DotWikiApi.Dtos;
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

        [HttpDelete("Snapshot/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            var snapshot = await _snapshotRepository.GetSnapshot(id);
            if (snapshot == null)
            {
                return NotFound();
            }
            if (snapshot.ApplicationUserId != currentUser.Id)
            {
                return Forbid();
            }
            _snapshotRepository.DeleteSnapshot(snapshot);
            return NoContent();
        }
        // TODO : make Rollback to a previous snapshot
    }
}