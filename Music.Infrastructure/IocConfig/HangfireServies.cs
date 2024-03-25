using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Music.Infrastructure.IocConfig;

public static class HangfireServies
{
	public static IServiceCollection AddHangfireServies(this IServiceCollection Services)
	{
		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
		string HangfireConnection = config["ConnectionStrings:MusicDBConecnection"];

		Services.AddHangfire(configuration => configuration
			.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UseSqlServerStorage(HangfireConnection));
		// Add the processing server as IHostedService
		Services.AddHangfireServer();

		return Services;
	}
}