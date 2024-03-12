using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Music.Application.Utilities;

public static class JWTGenerator
{
	public static string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public static string CreateToken(List<Claim> authClaims)
	{
		IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
		IConfigurationRoot root = builder.Build();

		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(root["JWT:Secret"]));
		_ = int.TryParse(root["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

		var tokenOption = new JwtSecurityToken(
			issuer: root["JWT:ValidIssuer"],
			audience: root["JWT:ValidAudience"],
			expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);

		string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);

		return tokenString;
	}
}
