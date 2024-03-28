using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Music.Domain.Entities;

namespace Music.Presentation.Controllers;

public class UsersController : BaseController
{
	private readonly UserManager<ApplicationUser> userManager;
	private readonly IMapper mapper;

	public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper)
	{
		this.mapper = mapper;
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
