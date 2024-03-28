using Microsoft.Extensions.DependencyInjection;
using Music.Application.Interface.Logic;
using Music.Application.Services;

namespace Music.Infrastructure.IocConfig;

public static class LogicServies
{
	public static IServiceCollection AddLogicServies(this IServiceCollection Services)
	{
		Services.AddScoped<IPaymentServisec, PaymentServisec>();
		Services.AddScoped<IJWTManager, JWTManager>();
		return Services;
	}
}