using AutoMapper;
using Music.Application.DataTransferObjects;
using Music.Domain.Entities;

namespace Music.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Singer, SingerDTO>();
        CreateMap<MusicEntity, MusicDTO>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<Subscription, SubscriptionDTO>();
    }
}
