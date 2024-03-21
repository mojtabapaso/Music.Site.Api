using Asp.Versioning;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Music.Application.Interface;
using Microsoft.AspNetCore.Identity;
using Music.Domain.Entities;

namespace Music.Controllers;


[ApiVersion("1", Deprecated = false)]
public class MusicController : BaseController
{
	private readonly IMusicServices musicServices;
	private readonly UserManager<ApplicationUser> userManager;

	public MusicController(IMusicServices musicServices, UserManager<ApplicationUser> userManager)
	{
		this.musicServices = musicServices;
		this.userManager = userManager;
	}

	[HttpGet("GetAll/Premium")]
	[Authorize("PremiumUser", AuthenticationSchemes = "Bearer")]
	public async Task<IActionResult> GetListMusicPremiumOnlyUser(int pageNumber, int pageSize)
	{
		var musics = await musicServices.GetSubscriptionRequiredMusicAsync(pageNumber, pageSize);
		return Ok(musics);
	}

	[HttpGet("GetAll")]
	public async Task<IActionResult> GetListMusic(int pageNumber, int pageSize)
	{
		var musics = await musicServices.GetSubscriptionNotRequiredMusicAsync(pageNumber, pageSize);
		return Ok(musics);
	}
	[HttpGet("Premium/{id}")]
	[ResponseCache(Duration = 60)]
	[Authorize("PremiumUser", AuthenticationSchemes = "Bearer")]
	public async Task<IActionResult> GetPremiumMusic(string id)
	{
		var music = await musicServices.FindSubscriptionRequiredMusicAsync(id);
		return Ok(music);
	}
	[HttpGet("{id}")]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> GetMusic(string id)
	{
		var music = await musicServices.FindSubscriptionNotRequiredMusicAsync(id);
		return Ok(music);
	}
	[HttpGet("Search")]
	public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = "")
	{
		var query = await musicServices.PaginationMusicAsync(filter, page, pageSize);
		var result = new
		{
			TotalCount = query.totalCount,
			TotalPages = query.totalPages,
			CurrentPage = page,
			PageSize = pageSize,
			Musices = query.musicEntities
		};
		return Ok(result);
	}
	[HttpGet("Singer/{id}")]//GetMusicListBySinger
	public async Task<IActionResult> GetMusicListBySinger(string singerId)
	{
		var musics = await musicServices.GetMusicListBySingerAsync(singerId);
		return Ok(musics);
	}
	[HttpGet("Category/{categoryName}")]
	public async Task<IActionResult> GetMusicListByCategory(string categoryName)
	{
		var musics = await musicServices.GetMusicListByCategoryAsync(categoryName);
		return Ok(musics);

	}

}