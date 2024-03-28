using AutoMapper;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Music.Application.Interface.Entity;
using Music.Application.DataTransferObjects;

namespace Music.Presentation.Controllers;

public class SingerController : BaseController
{
	private readonly IMapper mapper;
	private readonly ISingerServices singerServices;

	public SingerController(IMapper mapper, ISingerServices singerServices)
	{
		this.mapper = mapper;
		this.singerServices = singerServices;
	}
	[HttpGet("List")]
	public async Task<IActionResult> GetListSinger()
	{
		var singers = await singerServices.GetAllAsync();
		if (singers == null)
		{
			return NotFound();
		}
		var singersDto = mapper.Map<List<SingerDTO>>(singers);
		return Ok(singersDto);
	}
	[HttpGet("{id}")]
	public async Task<IActionResult> GetDetailSinger(string id)
	{
		var singer = await singerServices.FindByIdAsync(id);
		if (singer == null)
		{
			return NotFound();
		}
		var singerDto = mapper.Map<SingerDTO>(singer);
		return Ok(singerDto);
	}
}
