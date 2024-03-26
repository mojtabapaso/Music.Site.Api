using AutoMapper;
using Api.Controllers;

namespace Music.Presentation.Controllers;

public class SingerController : BaseController
{
	private readonly IMapper mapper;

	public SingerController(IMapper mapper)
    {
		this.mapper = mapper;
	}

}
