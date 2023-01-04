using AutoMapper;
using DotWikiApi.Dtos;
using DotWikiApi.Models;

namespace DotWikiApi.Profiles;

public class SnapshotProfile: Profile
{
    public SnapshotProfile()
    {
        CreateMap<Snapshot,SnapshotReadDto>();
    }
}