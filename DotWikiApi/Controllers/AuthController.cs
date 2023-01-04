using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DotWikiApi.Dtos;
using DotWikiApi.Services.Auth;
using DotWikiApi.Services.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotWikiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMailService _mailService;
    private readonly IOptions<MailSettings> _options;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMailService mailService,
        IOptions<MailSettings> mailSettings,
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _mailService = mailService;
        _options = mailSettings;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var (result, user) = await _authService.RegisterUser(registerDto);
        if ((result,user) == (null, null))
        {
            return BadRequest(new { Message = "User already exists" });
        }

        try
        {
            await _mailService.SendEmailAsync(new SignupMail(_options, user));
        }
        catch (Exception e)
        {
            _logger.LogError("{}",e.Message);
        }

        return !result.Succeeded
            ? BadRequest(new { result.Errors })
            : Ok();
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto model)
    {
        var token = await _authService.GenerateJwt(model);
        if (token == null)
        {
            return Unauthorized();
        }

        return new TokenDto(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }
}