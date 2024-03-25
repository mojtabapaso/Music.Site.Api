using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Music.Application.Interface.Entity;
using Music.Domain.Entities;
using Music.Infrastructure.Context;

namespace Music.Infrastructure.Services;
public class MusicServices : GenericServices<MusicEntity>, IMusicServices
{
	private readonly MusicDBContext context;
	public MusicServices(MusicDBContext context) : base(context)
	{
		this.context = context;
	}
	public async Task<(List<MusicEntity> musicEntities, int totalCount, int totalPages)> PaginationMusicAsync(string filter, int page = 1, int pageSize = 10)
	{
		var query = context.Musics.Where(option => option.Subtitle.Contains(filter) || option.Singer.Name.Contains(filter));
		var totalCount = await query.CountAsync();
		var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
		query = query.Skip((page - 1) * pageSize).Take(pageSize);
		var result = (await query.ToListAsync(), totalCount, totalPages);
		return result;
	}
	public async Task<List<MusicEntity>> GetSubscriptionRequiredMusicAsync(int pageNumber, int pageSize)
	{
		var query = context.Musics.Where(c => c.NeedSubscription == true);

		int totalItems = await query.CountAsync();
		int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

		var musics = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

		return musics;
	}
	public async Task<List<MusicEntity>> GetSubscriptionNotRequiredMusicAsync(int pageNumber, int pageSize)
	{
		var query = context.Musics.Where(c => c.NeedSubscription == false);

		int totalItems = await query.CountAsync();
		int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

		var musics = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

		return musics;
	}
	public async Task<MusicEntity> FindSubscriptionNotRequiredMusicAsync(string id)
	{
		var music = await context.Musics.Where(m => m.NeedSubscription == false && m.Id == id).FirstAsync();
		return music;
	}
	public async Task<MusicEntity> FindSubscriptionRequiredMusicAsync(string id)
	{
		var music = await context.Musics.Where(m => m.NeedSubscription == true && m.Id == id).FirstAsync();
		return music;
	}

	public async Task<List<MusicEntity>> GetMusicListBySingerAsync(string singerId)
	{
		var musics = await context.Musics.Where(m=>m.Singer.Id == singerId).ToListAsync();
		return musics;
	}

	public async Task<List<MusicEntity>> GetMusicListByCategoryAsync(string categoryName)
	{
		var musics = await context.Musics.Where(m => m.MusicCategories.Any(mc=>mc.Title == categoryName)).ToListAsync();
		return musics;
	}
}