using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Music.Infrastructure.Data.Context;

namespace Music.Infrastructure.IocConfig;

public static class DataBaseContextServies
{
	public static IServiceCollection AddDbContextServies(this IServiceCollection Services)
	{
		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

		string connectionString = config["ConnectionStrings:MusicDBConecnection"];

		Services.AddDbContext<MusicDBContext>(option =>
		{
			option.UseSqlServer(connectionString);
		});
		return Services;
	}
}