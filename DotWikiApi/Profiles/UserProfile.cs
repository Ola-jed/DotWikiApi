using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Models;

namespace DotWikiApi.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser,UserReadDto>();
        }
    }
}