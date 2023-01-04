using System.Collections.Generic;
using System.Threading.Tasks;
using DotWikiApi.Dtos;
using DotWikiApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotWikiApi.Services.User;

public class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> FindUserById(string id)
    {
        return await _userManager
            .Users
            .Include(usr => usr.Articles)
            .FirstOrDefaultAsync(usr => usr.Id == id);
    }

    public async Task<ApplicationUser?> FindUserByUserName(string userName)
    {
        return await _userManager
            .Users
            .Include(usr => usr.Articles)
            .FirstOrDefaultAsync(usr => usr.UserName == userName);
    }

    public async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
    {
        return await _userManager.GetRolesAsync(applicationUser);
    }

    public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user,password);
    }

    public async Task<IdentityResult> UpdateUser(ApplicationUser initialValue, AccountUpdateDto updateDto)
    {
        initialValue.Email = updateDto.Email;
        initialValue.UserName = updateDto.Username;
        return await _userManager.UpdateAsync(initialValue);
    }

    public async Task<IdentityResult> DeleteUser(ApplicationUser user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task<bool> CheckPassword(string username, string password)
    {
        var user = await FindUserByUserName(username);
        if (user == null)
        {
            return false;
        }
        
        return await _userManager.CheckPasswordAsync(user, password);
    }
}