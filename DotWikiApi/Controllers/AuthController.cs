using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using DotWikiApi.Services.Auth;
using DotWikiApi.Services.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DotWikiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IOptions<MailSettings> _options;

        public AuthController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IMailService mailService,
            IOptions<MailSettings> mailSettings,
            IAuthService authService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _options = mailSettings;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var registerData = await _authService.RegisterUser(registerDto);
            if (registerData == (null, null))
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User already exists!" });
            }

            var user = registerData.Item2;
            var result = registerData.Item1;
            try
            {
                await _mailService.SendEmailAsync(new SignupMail(_options, user));
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }

            return !result.Succeeded
                ? StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again.",
                        Errors = result.Errors
                    })
                : Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var token = await _authService.GenerateJwt(model);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}