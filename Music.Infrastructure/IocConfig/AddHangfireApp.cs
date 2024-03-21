using Hangfire;
using Microsoft.AspNetCore.Builder;
using HangfireBasicAuthenticationFilter;
using Music.Application.Interface;

namespace Music.Infrastructure.IocConfig;

public static class HangfireApp
{
	public  static IApplicationBuilder AddHangfireApp(this IApplicationBuilder app)
	{
		app.UseHangfireDashboard();
		RecurringJob.AddOrUpdate<ISubscriptionServices>(cornJob => cornJob.SubscriptionExpirationCheckerAsync(), "30 22 * * *");
	
		// equivalent to 3 nights Tehran time

		app.UseHangfireDashboard("/hangfire", new DashboardOptions
		{
			Authorization = new[]
		{
				new HangfireCustomBasicAuthenticationFilter
		{
			//User = Configuration.GetSection("HangfireSettings:Username").Value,
			//Pass = app.Configuration.GetSection("HangfireSettings:Password").Value
			User = "Admin",
			Pass = "Admin"
		}}
		});
		return app;
	}
}
