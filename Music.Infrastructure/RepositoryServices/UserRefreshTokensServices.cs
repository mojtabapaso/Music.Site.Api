using Music.Application.Interface.Logic;
using Music.Domain.Entities;
using Music.Infrastructure.Context;

namespace Music.Infrastructure.Services;

public class UserRefreshTokensServices : GenericServices<UserRefreshTokens>, IUserRefreshTokensServices
{
	public UserRefreshTokensServices(MusicDBContext context) : base(context)
	{
	}
}