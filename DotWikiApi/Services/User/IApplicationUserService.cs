using System.Collections.Generic;
using System.Threading.Tasks;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity;

namespace DotWikiApi.Services.User;

public interface IApplicationUserService
{
    Task<ApplicationUser?> FindUserById(string id);
    Task<ApplicationUser?> FindUserByUserName(string userName);
    Task<IList<string>> GetUserRoles(ApplicationUser applicationUser);
    Task<IdentityResult> CreateUser(ApplicationUser user, string password);
    Task<IdentityResult> UpdateUser(ApplicationUser initialValue, AccountUpdateDto updateDto);
    Task<IdentityResult> DeleteUser(ApplicationUser user);
    Task<bool> CheckPassword(string username, string password);
}