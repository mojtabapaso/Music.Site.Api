using Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Domain.Entities;

namespace Music.Presentation.Controllers;

public class UsersController : BaseController
{
	private readonly UserManager<ApplicationUser> userManager;

	public UsersController(UserManager<ApplicationUser> userManager)
    {
		this.userManager = userManager;
	}
	[HttpGet("{id}")]
	[Authorize]
	public async Task<IActionResult> DetailUser(string id)
	{
		var user = await userManager.FindByIdAsync(id);
		if (user == null)
		{
			return NotFound();
		}
		return Ok(user);
	}
}
