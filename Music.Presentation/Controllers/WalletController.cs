using Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Application.Interface.Entity;
using Music.Domain.Entities;

namespace Music.Presentation.Controllers;

public class WalletController : BaseController
{
	private readonly IWalletServices walletServices;
	private readonly UserManager<ApplicationUser> userManager;

	public WalletController(IWalletServices walletServices, UserManager<ApplicationUser> userManager)
	{
		this.walletServices = walletServices;
		this.userManager = userManager;
	}
	[HttpPost("Charg")]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public async Task<IActionResult> ChargingWallet(UInt16 amount)
	{
		// TODO payment required
		if (amount < 0)
		{
			return BadRequest();
		}
		var user = await userManager.GetUserAsync(User);
		var existWallet = await walletServices.GetWalletByUserIdAsync(user.Id);
		if (existWallet != null)
		{
			existWallet.Amount += amount;
			walletServices.Update(existWallet);
			return Ok();
		}
		var wallet = new Wallet
		{
			Id = Guid.NewGuid().ToString(),
			User = user,
			IdUser = user.Id,
			Amount = amount,
		};
		await walletServices.AddAsync(wallet);
		return Ok();
	}
}