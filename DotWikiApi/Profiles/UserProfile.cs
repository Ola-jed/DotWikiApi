using AutoMapper;
using DotWikiApi.Authentication;
using DotWikiApi.Dtos;

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