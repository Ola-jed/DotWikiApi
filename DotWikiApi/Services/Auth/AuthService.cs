using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace DotWikiApi.Services.Auth
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUserCredentials(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            return user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password);
        }

        public async Task<(IdentityResult, ApplicationUser)> RegisterUser(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
            {
                return (null, null);
            }
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username,
                RegisterDate = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return (result, user);
        }

        public async Task<JwtSecurityToken> GenerateJwt(LoginDto loginDto)
        {
            if (! await ValidateUserCredentials(loginDto))
            {
                return null;
            }
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience:_configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}