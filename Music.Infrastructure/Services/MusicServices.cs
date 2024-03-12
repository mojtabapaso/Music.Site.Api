using Music.Domain.Entities;
using Music.Infrastructure.Data.Context;
using Music.Infrastructure.Interface;
using Music.Infrastructure.Services;

namespace Music.Infrastructure.Services;

public class MusicServices : GenericServices<MusicEntity>, IMusicServices
{
	public MusicServices(MusicDBContext context) : base(context)
	{
	}

}
