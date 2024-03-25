using Music.Application.Interface.Entity;
using Music.Domain.Entities;
using Music.Infrastructure.Context;

namespace Music.Infrastructure.Services;

public class CategoryServices : GenericServices<Category>, ICategoryServices
{
	private readonly MusicDBContext context;
	public CategoryServices(MusicDBContext context) : base(context)
	{
		this.context = context;

	}

}
