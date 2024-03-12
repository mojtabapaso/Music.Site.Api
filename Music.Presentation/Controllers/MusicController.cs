using Api.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Music.Infrastructure.Interface;

namespace Music.Controllers;

[ApiVersion("1", Deprecated = false)]
public class MusicController : BaseController
{
	private readonly IMusicServices musicServices;
	
	public MusicController(IMusicServices musicServices)
	{
		this.musicServices = musicServices;
	}
	[HttpGet("GetAll")]
	public async Task<IActionResult> GetListMusic()
	{
		var musics = await musicServices.GetAllAsync();
		return Ok(musics);
	}
	[HttpGet("{id}")]
	[ResponseCache(Duration = 60)]
	public async Task<IActionResult> GetMusic([FromBody] string id)
	{
		var music = await musicServices.FindByIdAsync(id);
		return new ObjectResult(music);
		//return Ok();
	}


}