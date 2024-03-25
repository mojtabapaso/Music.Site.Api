using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Music.Application.Interface.Logic;
using Music.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Music.Application.Services;

public class JWTManager : IJWTManager
{
	private readonly IConfiguration configuration;
	private readonly UserManager<ApplicationUser> userManager;
	private readonly SymmetricSecurityKey secretKey;
	private readonly string ValidIssuer;
	private readonly string ValidAudience;
	private readonly string SecretKey;
	private readonly string ExpirationMinutes;
	private readonly string ExpirationsRefreshToken;

	public JWTManager(IConfiguration configuration, UserManager<ApplicationUser> userManager)
	{
		this.configuration = configuration;
		ValidAudience = configuration["Jwt:ValidAudience"];
		ValidIssuer = configuration["Jwt:ValidIssuer"];
		SecretKey = configuration["Jwt:SecretKey"];
		ExpirationMinutes = configuration["Jwt:ExpirationMinutes"];
		ExpirationsRefreshToken = configuration["Jwt:ExpirationsRefreshToken"];
		this.userManager = userManager;
		secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
	}
	public async Task<string> GenerateAccessTokenAsync(string userName)
	{
		var user = await userManager.FindByNameAsync(userName);
		var userClaims = await userManager.GetClaimsAsync(user);
		var tokenOptions = new JwtSecurityToken(
			issuer: ValidIssuer,
			audience: ValidIssuer,
			claims: userClaims,
			expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(ExpirationMinutes)),
			signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
		);
		string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		return accessToken;
	}
	public async Task<string> GenerateRefreshTokenAsync(string userName)
	{
		var user = await userManager.FindByNameAsync(userName);

		var userClaims = await userManager.GetClaimsAsync(user);
		var tokenOptions = new JwtSecurityToken(
			issuer: ValidIssuer,
			audience: ValidAudience,
			claims: userClaims,
			expires: DateTime.UtcNow.AddMonths(Convert.ToByte(ExpirationsRefreshToken)),
			signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
		);
		string RefreshToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		return RefreshToken;
	}
	public ClaimsPrincipal GetPrincipalFromToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var validationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = secretKey,
			ValidateIssuer = true,
			ValidIssuer = ValidIssuer,
			ValidateAudience = true,
			ValidAudience = ValidIssuer,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero
		};

		SecurityToken validatedToken;
		return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
	}
	public bool VerifyToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		try
		{
			var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = secretKey,
				ValidateIssuer = true,
				ValidIssuer = ValidIssuer,
				ValidateAudience = true,
				ValidAudience = ValidIssuer,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}