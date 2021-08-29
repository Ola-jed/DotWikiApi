using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Data;
using DotWikiApi.Dtos;
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

        public SnapshotController(ISnapshotRepository snapshotRepository,
            IMapper mapper,
            IArticleRepository articleRepository)
        {
            _snapshotRepository = snapshotRepository;
            _articleRepository = articleRepository;
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

        // TODO : make delete and rollback to a previous snapshot
    }
}