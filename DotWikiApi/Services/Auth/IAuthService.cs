using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using DotWikiApi.Dtos;

namespace DotWikiApi.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> ValidateUserCredentials(LoginDto loginDto);
        Task<JwtSecurityToken> GenerateJwt(LoginDto loginDto);
    }
}