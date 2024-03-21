using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Music.Application.Interface;

namespace Music.Presentation.Controllers;

public class CategoryController : BaseController
{
	private readonly ICategoryServices categoryServices;

	public CategoryController(ICategoryServices categoryServices)
	{
		this.categoryServices = categoryServices;
	}
	public async Task<IActionResult> GetListCategory()
	{
		var category = await categoryServices.GetAllAsync();
		return Ok(category);
	}
}
