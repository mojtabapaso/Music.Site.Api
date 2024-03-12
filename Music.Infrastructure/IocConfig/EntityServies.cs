using Microsoft.Extensions.DependencyInjection;
using Music.Infrastructure.Interface;
using Music.Infrastructure.Services;

namespace Music.Infrastructure.IocConfig;

public static class EntityServies
{
    public static IServiceCollection AddEntityServies(this IServiceCollection Services)
    {
        Services.AddScoped<IMusicServices, MusicServices>();
        return Services;
    }
}
