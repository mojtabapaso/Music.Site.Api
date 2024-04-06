using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Application.DataTransferObjects;
using Music.Application.Interface.Entity;
using Music.Application.Interface.Logic;
using Music.Domain.Entities;
using Music.Infrastructure.Services;
using System.Security.Claims;

namespace Music.Presentation.Controllers;

public class SubscriptionController : BaseController
{
	private readonly ISubscriptionServices subscriptionServices;
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IUserRefreshTokensServices userRefreshTokensServices;
	private readonly IPaymentServisec paymentServisec;
	private readonly IWalletServices walletServices;
	private readonly IConfiguration configuration;
    private readonly IMapper mapper;

    public SubscriptionController(ISubscriptionServices subscriptionServices,
		UserManager<ApplicationUser> userManager,
		IUserRefreshTokensServices userRefreshTokensServices,
		IPaymentServisec paymentServisec,
		IWalletServices walletServices,
		IConfiguration configuration,
		IMapper mapper
		)
	{
		this.subscriptionServices = subscriptionServices;
		this.userManager = userManager;
		this.userRefreshTokensServices = userRefreshTokensServices;
		this.paymentServisec = paymentServisec;
		this.walletServices = walletServices;
		this.configuration = configuration;
        this.mapper = mapper;
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
		var subscriptionDto =  mapper.Map<SubscriptionDTO>(subscription);
		return Ok(subscriptionDto);
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


	[HttpPost("Set/Subscription/Wallet")]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public async Task<IActionResult> BuySubscriptionByWallet([FromForm] int month)
	{
		//only 1 | 3 | 6 | 12 Months
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

		var wallet = await walletServices.GetWalletByUserIdAsync(user.Id);

		if (wallet is null)
		{
			return BadRequest("Wallet is empty");
		}

		if (wallet.Amount < UInt32.Parse(subscriptionPrice))
		{
			return BadRequest("The wallet does not have enough balance for this transaction");
		};

		wallet.Amount = wallet.Amount - UInt32.Parse(subscriptionPrice);

		walletServices.Update(wallet);
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
}
