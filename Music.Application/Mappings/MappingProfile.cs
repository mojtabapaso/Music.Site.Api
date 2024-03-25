using AutoMapper;
using Music.Application.DataTransferObjects;
using Music.Domain.Entities;

namespace Shop.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<MusicEntity, MusicDTO>();
		CreateMap<Category, CategoryDTO>();
		CreateMap<Singer, SingerDTO>();
	}

}
