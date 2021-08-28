using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Models;

namespace DotWikiApi.Profiles
{
    public class ArticleProfile: Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleCreateDto,Article>();
            CreateMap<ArticleUpdateDto,Article>();
            CreateMap<Article,ArticleReadDto>();
        }
    }
}