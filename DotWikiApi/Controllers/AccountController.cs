using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Authentication;
using DotWikiApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAccount(string id)
        {
            var usr = await _userManager
                .Users
                .Include(usr => usr.Articles)
                .FirstOrDefaultAsync(usr => usr.Id == id);
            return usr == null ? NotFound() : Ok(_mapper.Map<UserReadDto>(usr));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var usr = await _userManager
                .Users
                .Include(usr => usr.Articles)
                .FirstOrDefaultAsync(usr => usr.UserName == HttpContext.User.Identity.Name);
            return Ok(_mapper.Map<UserReadDto>(usr));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put(AccountUpdateDto accountUpdateDto)
        {
            var usr = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            usr.Email = accountUpdateDto.Email;
            usr.UserName = accountUpdateDto.Username;
            // TODO : Finish implementation
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public ActionResult Delete()
        {
            // TODO : Finish implementation
            return NoContent();
        }
    }
}