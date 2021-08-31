using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Services.Auth;
using DotWikiApi.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotWikiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IApplicationUserService _userService;

        public AccountController(IMapper mapper,
            IAuthService authService,
            IApplicationUserService userService)
        {
            _mapper = mapper;
            _authService = authService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAccount(string id)
        {
            var usr = await _userService.FindUserById(id);
            return usr == null ? NotFound() : Ok(_mapper.Map<UserReadDto>(usr));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            return Ok(_mapper.Map<UserReadDto>(usr));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Put(AccountUpdateDto accountUpdateDto)
        {
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            if (usr == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var loginDto = new LoginDto() { Username = usr.UserName, Password = accountUpdateDto.Password };
            var validCredentials = await _authService.ValidateUserCredentials(loginDto);
            if (!validCredentials)
            {
                return Unauthorized();
            }

            var res = await _userService.UpdateUser(usr, accountUpdateDto);
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
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            var validCredentials = await _authService.ValidateUserCredentials(loginDto);
            if (!validCredentials || usr == null)
            {
                return Unauthorized();
            }

            var res = await _userService.DeleteUser(usr);
            return res.Succeeded
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    res.Errors
                });
        }
    }
}