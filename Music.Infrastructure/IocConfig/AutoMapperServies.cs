using Microsoft.Extensions.DependencyInjection;
using Music.Application.Mappings;

namespace Music.Infrastructure.IocConfig;
public static class AutoMapperServies
{
	public static IServiceCollection AddAutoMapperServies(this IServiceCollection Services)
	{
		Services.AddAutoMapper(typeof(MappingProfile));
		return Services;
	}
}
