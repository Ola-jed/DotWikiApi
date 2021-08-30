using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Authentication;
using DotWikiApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotWikiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/me")]
        public async Task<ActionResult> Get()
        {
            var usr = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            return Ok(_mapper.Map<UserReadDto>(usr));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAccount(string id)
        {
            var usr = await _userManager.FindByIdAsync(id);
            return usr == null ? NotFound() : Ok(usr);
        }
    }
}