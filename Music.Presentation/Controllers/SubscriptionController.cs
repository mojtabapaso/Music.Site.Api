using Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Application.Interface.Entity;
using Music.Application.Interface.Logic;
using Music.Domain.Entities;
using System.Security.Claims;

namespace Music.Presentation.Controllers;

public class SubscriptionController : BaseController
{
	private readonly ISubscriptionServices subscriptionServices;
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IUserRefreshTokensServices userRefreshTokensServices;

	public SubscriptionController(ISubscriptionServices subscriptionServices, UserManager<ApplicationUser> userManager, IUserRefreshTokensServices userRefreshTokensServices)
	{
		this.subscriptionServices = subscriptionServices;
		this.userManager = userManager;
		this.userRefreshTokensServices = userRefreshTokensServices;
	}
	[HttpGet("Get/Subscription")]
	[Authorize("PremiumUser", AuthenticationSchemes = "Bearer")]

	//for set in profile user
	public async Task<IActionResult> GetSubscription()
	{
		var userId = userManager.GetUserId(User);
		var subscription = await subscriptionServices.GetSubscriptionByUserIdAsync(userId);
		if (subscription == null)
		{
			return NotFound();
		}
		return Ok(subscription);
	}

	[HttpPost("Set/Subscription")]
	[Authorize(AuthenticationSchemes = "Bearer")]
	//[Authorize("PremiumUser",AuthenticationSchemes = "Bearer")]

	public async Task<IActionResult> BuySubscription([FromForm] int month)
	{
		var user = await userManager.GetUserAsync(User);
		if (user == null)
		{
			return BadRequest();
		}

		// TODO Payment 
		// and validate suuccess then

		DateTime dateTime = DateTime.UtcNow;
		DateTime dateTimeMonth = DateTime.UtcNow.AddMonths(month);
		//Claim claim = new Claim("IsPremiumUser", "true", ClaimValueTypes.Boolean);
		Claim claim = new Claim("SpecialAccess", "true", ClaimValueTypes.Boolean);
		//await userManager.AddToRoleAsync(user, "PremiumUser");
		//await userManager.AddClaimAsync(user, new Claim("SpecialAccess", "true", ClaimValueTypes.Boolean));

		var result = await userManager.AddClaimAsync(user, claim);
		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			var rr = ModelState;
			return BadRequest(ModelState);
		}
		var existSubscription = await subscriptionServices.GetSubscriptionByUserIdAsync(user.Id);
		if (existSubscription == null)
		{
			var newSubscription = new Subscription
			{
				Id = Guid.NewGuid().ToString(),
				IsActive = true,
				Expired = dateTimeMonth,
			};
			await subscriptionServices.AddAsync(newSubscription);
		}
		if (existSubscription.Expired < DateTime.UtcNow)
		{
			existSubscription.Expired = dateTimeMonth;
			subscriptionServices.Update(existSubscription);

		}
		if (existSubscription.Expired > DateTime.UtcNow)
		{
			existSubscription.Expired.AddMonths(month);
			subscriptionServices.Update(existSubscription);

		}
		return Ok();
	}

}
