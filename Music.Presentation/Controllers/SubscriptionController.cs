using Api.Controllers;
using IdempotentAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Application.Interface.Entity;
using Music.Application.Interface.Logic;
using Music.Application.Services;
using Music.Domain.Entities;
using System.Security.Claims;

namespace Music.Presentation.Controllers;

public class SubscriptionController : BaseController
{
	private readonly ISubscriptionServices subscriptionServices;
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IUserRefreshTokensServices userRefreshTokensServices;
	private readonly IPaymentServisec paymentServisec;
	private readonly IConfiguration configuration;

	public SubscriptionController(ISubscriptionServices subscriptionServices,
		UserManager<ApplicationUser> userManager,
		IUserRefreshTokensServices userRefreshTokensServices,
		IPaymentServisec paymentServisec,
		IConfiguration configuration
		)
	{
		this.subscriptionServices = subscriptionServices;
		this.userManager = userManager;
		this.userRefreshTokensServices = userRefreshTokensServices;
		this.paymentServisec = paymentServisec;
		this.configuration = configuration;
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
	public async Task<IActionResult> BuySubscription([FromForm] int month)
	{
		//only 1 | 3 | 6 | 12
		var user = await userManager.GetUserAsync(User);
		if (user == null)
		{
			return BadRequest();
		}

		string subscriptionPriceKey = $"SubscriptionPrice:Mounth_{month}";
		string subscriptionPrice = configuration.GetValue<string>(subscriptionPriceKey);
		if (string.IsNullOrEmpty(subscriptionPrice))
		{
			return BadRequest("Invalid subscription duration.");
		}

		var statusPayment = await paymentServisec.PaymentAsync(subscriptionPrice, "/");
		if (statusPayment.IsSuccessStatusCode)
		{

			DateTime dateTime = DateTime.UtcNow;
			DateTime dateTimeMonth = DateTime.UtcNow.AddMonths(month);
			Claim claim = new Claim("IsPremiumUser", "true", ClaimValueTypes.Boolean);
			await userManager.AddToRoleAsync(user, "PremiumUser");
			await userManager.AddClaimAsync(user, new Claim("SpecialAccess", "true", ClaimValueTypes.Boolean));

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
		return BadRequest();

	}

}
