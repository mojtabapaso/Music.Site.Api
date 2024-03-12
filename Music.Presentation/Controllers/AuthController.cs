using Api.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Application.DataTransferObjects;
using Music.Application.Utilities;
using Music.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Music.WebAPI.Controllers;

//[ApiController]
//[Route("api/[controller]")]
public class AuthController : BaseController
{
	private readonly IConfiguration configuration;
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IPasswordHasher<ApplicationUser> passwordHasher;

	public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager , IPasswordHasher<ApplicationUser> passwordHasher)
	{
		this.configuration = configuration;
		this.userManager = userManager;
		this.passwordHasher = passwordHasher;
	}

	[HttpPost("Register")]
	public async Task<IActionResult> Register([FromForm] RegisterDTO register)
	{
		var user = await userManager.FindByNameAsync(register.UserName);
		if (user != null)
		{
			return Conflict();
		}
		string password = passwordHasher.HashPassword(user, register.Password);
		var appUser = new ApplicationUser
		{
			Id = Guid.NewGuid().ToString(),
			UserName = register.UserName,
			PasswordHash = password,
		};
		await userManager.CreateAsync(appUser);
		
		var authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, register.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};
		foreach (var claim in authClaims)
		{
			await userManager.AddClaimAsync(appUser, claim);
		}
		var newAccessToken = JWTGenerator.CreateToken(authClaims);
		var tokennewRefreshToken = JWTGenerator.GenerateRefreshToken();

		return Ok(new
		{
			AccessToken = newAccessToken,
			RefreshToken = tokennewRefreshToken,
		});
	}


	[HttpPost("Login")]
	public async Task<IActionResult> Login([FromForm] LoginDTO login)
	{
		var user = await userManager.FindByNameAsync(login.UserName);
		if (user == null)
		{
			return NotFound();
		}
		var check = await userManager.CheckPasswordAsync(user, login.Password);
		if (check == false)
		{
			return BadRequest("Password is invalid");
		}
		var userRoles = await userManager.GetRolesAsync(user);

		var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};
		foreach (var userRole in userRoles)
		{
			authClaims.Add(new Claim(ClaimTypes.Role, userRole));
		}

		var newAccessToken = JWTGenerator.CreateToken(authClaims);
		var tokennewRefreshToken = JWTGenerator.GenerateRefreshToken();

		return Ok(new
		{
			AccessToken = newAccessToken,
			RefreshToken = tokennewRefreshToken,
		});
	}
}
