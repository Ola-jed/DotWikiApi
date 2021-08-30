using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity;

namespace DotWikiApi.Services.Auth
{
    public interface IAuthService
    {
        Task<(IdentityResult,ApplicationUser)> RegisterUser(RegisterDto registerDto);
        Task<bool> ValidateUserCredentials(LoginDto loginDto);
        Task<JwtSecurityToken> GenerateJwt(LoginDto loginDto);
    }
}