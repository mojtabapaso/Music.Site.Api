using Api.Controllers;
using AutoMapper;
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
		var categories = await categoryServices.GetAllAsync();
		var categoriesDto = mapper.Map<List<CategoryDTO>>(categories);
		return Ok(categoriesDto);
	}
}
