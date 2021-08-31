using System;
using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using DotWikiApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IAuthService authService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authService = authService;
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
            var usr = await _userManager.GetUserAsync(HttpContext.User);
            Console.WriteLine(usr == null);
            var loginDto = new LoginDto() { Username = usr.UserName, Password = accountUpdateDto.Password };
            var validCredentials = await _authService.ValidateUserCredentials(loginDto);
            if (!validCredentials)
            {
                return Unauthorized();
            }
            usr.Email = accountUpdateDto.Email;
            usr.UserName = accountUpdateDto.Username;
            var res = await _userManager.UpdateAsync(usr);
            return res.Succeeded
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Errors = res.Errors
                });
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(LoginDto loginDto)
        {
            var usr = await _userManager.FindByNameAsync(HttpContext.User.Identity?.Name);
            var validCredentials = await _authService.ValidateUserCredentials(loginDto);
            if (!validCredentials || usr == null)
            {
                return Unauthorized();
            }
            var res = await _userManager.DeleteAsync(usr);
            return res.Succeeded
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Errors = res.Errors
                });
        }
    }
}