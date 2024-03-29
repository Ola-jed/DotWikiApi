using System.Threading.Tasks;
using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Services.Auth;
using DotWikiApi.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotWikiApi.Controllers;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserReadDto>> GetAccount(string id)
    {
        var usr = await _userService.FindUserById(id);
        return usr == null ? NotFound() : Ok(_mapper.Map<UserReadDto>(usr));
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<UserReadDto?> Get()
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        return _mapper.Map<UserReadDto>(usr);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Put(AccountUpdateDto accountUpdateDto)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        if (usr == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var loginDto = new LoginDto { Username = usr.UserName, Password = accountUpdateDto.Password };
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
                res.Errors
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