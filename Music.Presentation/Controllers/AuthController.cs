using Api.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Application.DataTransferObjects;
using Music.Application.Interface;
using Music.Domain.Entities;
using System.Security.Claims;

namespace Music.WebAPI.Controllers;

public class AuthController : BaseController
{
	private readonly IConfiguration configuration;
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IPasswordHasher<ApplicationUser> passwordHasher;
	private readonly IJWTManager jWTManager;
	private readonly IUserRefreshTokensServices userRefreshTokensServices;

	public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, IJWTManager jWTManager, IUserRefreshTokensServices userRefreshTokensServices)
	{
		this.configuration = configuration;
		this.userManager = userManager;
		this.passwordHasher = passwordHasher;
		this.jWTManager = jWTManager;
		this.userRefreshTokensServices = userRefreshTokensServices;
	}

	[HttpPost("Register")]
	public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
	{
		var existingUser = await userManager.FindByNameAsync(registerDTO.UserName);
		if (existingUser != null)
		{
			return Conflict();
		}

		var newUser = new ApplicationUser
		{
			Id = Guid.NewGuid().ToString(),
			UserName = registerDTO.UserName,
		};

		var createUserResult = await userManager.CreateAsync(newUser, registerDTO.Password);
		if (!createUserResult.Succeeded)
		{
			var errors = createUserResult.Errors.Select(error => error.Description);
			return BadRequest(errors);
		}

		var userWithClaims = await userManager.FindByNameAsync(registerDTO.UserName);
		await userManager.AddClaimAsync(userWithClaims, new Claim("SpecialAccess", "true", ClaimValueTypes.Boolean));


		string accessToken = await jWTManager.GenerateAccessTokenAsync(registerDTO.UserName);
		string refreshToken = await jWTManager.GenerateRefreshTokenAsync(registerDTO.UserName);
		var tokens = new Tokens
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};

		var userRefreshTokens = new UserRefreshTokens
		{
			RefreshToken = tokens.RefreshToken,
			IsActive = true,
			UserName = registerDTO.UserName
		};

		await userRefreshTokensServices.AddAsync(userRefreshTokens);

		return Ok(tokens);
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
		var accessToken = await jWTManager.GenerateAccessTokenAsync(login.UserName);
		var refreshToken = await jWTManager.GenerateRefreshTokenAsync(login.UserName);
		var token = new Tokens()
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
		return Ok(token);
	}
}
