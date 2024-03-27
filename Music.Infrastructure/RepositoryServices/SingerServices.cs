using Music.Application.Interface.Entity;
using Music.Domain.Entities;
using Music.Infrastructure.Context;

namespace Music.Infrastructure.Services;

public class SingerServices : GenericServices<Singer>, ISingerServices
{
	private readonly MusicDBContext context;

	public SingerServices(MusicDBContext context) : base(context)
	{
		this.context = context;
	}

}