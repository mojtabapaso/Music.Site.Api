using Microsoft.Extensions.DependencyInjection;
using Music.Application.Interface.Entity;
using Music.Application.Interface.Logic;
using Music.Infrastructure.Services;

namespace Music.Infrastructure.IocConfig;

public static class RepositoryServies
{
    public static IServiceCollection AddRepositoryServies(this IServiceCollection Services)
    {
		Services.AddScoped<IMusicServices, MusicServices>();
		Services.AddScoped<ISubscriptionServices, SubscriptionServices>();
		Services.AddScoped<IUserRefreshTokensServices, UserRefreshTokensServices>();
		Services.AddScoped<ICategoryServices, CategoryServices>();
		Services.AddScoped<IWalletServices, WalletServices>();
		Services.AddScoped<ISingerServices, SingerServices>();
		return Services;
    }
}