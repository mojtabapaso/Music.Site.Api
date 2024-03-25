using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Music.Application.DataTransferObjects;
using Music.Application.Interface.Entity;

namespace Music.Presentation.Controllers;

public class CategoryController : BaseController
{
	private readonly ICategoryServices categoryServices;
	private readonly IMapper mapper;

	public CategoryController(ICategoryServices categoryServices,IMapper mapper)
	{
		this.categoryServices = categoryServices;
		this.mapper = mapper;
	}
	[HttpGet("List")]
	public async Task<IActionResult> GetListCategory()
	{
		var category = await categoryServices.GetAllAsync();
		var categoryDto = mapper.Map<CategoryDTO>(category);
		return Ok(categoryDto);
	}
}
