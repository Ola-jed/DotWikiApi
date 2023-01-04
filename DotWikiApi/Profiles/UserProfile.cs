using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Models;

namespace DotWikiApi.Profiles;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<LoginDto, ApplicationUser>();
        CreateMap<ApplicationUser,UserReadDto>();
        CreateMap<AccountUpdateDto, LoginDto>();
    }
}