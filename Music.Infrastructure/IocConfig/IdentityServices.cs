using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Music.Domain.Entities;
using Music.Infrastructure.Data.Context;
namespace Music.Infrastructure.IocConfig;

public static class IdentityServices
{
	public static IServiceCollection AddIdentityServies(this IServiceCollection Services)
	{
		Services.AddIdentity<ApplicationUser, IdentityRole>()
		.AddUserManager<UserManager<ApplicationUser>>()
		.AddRoleManager<RoleManager<IdentityRole>>().AddEntityFrameworkStores<MusicDBContext>();

		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

		string jwtKey = config["JWT:Secret"];
		string Issuer = config["JWT:ValidIssuer"];


		Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(option =>
		{
			option.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidateAudience = false,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = Issuer,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
			};
		});

		Services.AddCors(option =>
		{
			option.AddPolicy("EnableCors", builder =>
			{
				builder.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod()
				.Build();

			});
		});
		return Services;
	}
}
