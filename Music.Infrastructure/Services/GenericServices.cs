using Microsoft.EntityFrameworkCore;
using Music.Domain.Common;
using Music.Infrastructure.Data.Context;
using Music.Infrastructure.Interface;

namespace Music.Infrastructure.Services;

public class GenericServices<TEntity> : IGenericServices<TEntity> where TEntity : BaseEntity, new()
{
	private readonly MusicDBContext context;
	private DbSet<TEntity> _entity;
	public GenericServices(MusicDBContext context)
	{
		this.context = context;
		_entity = context.Set<TEntity>();
	}
	public void Add(TEntity entity)
		=> _entity.Add(entity);

	public async Task AddAsync(TEntity entity)
	   => await _entity.AddAsync(entity);
	public TEntity FindById(string id)
		=> _entity.Find(id);

	public async Task<TEntity> FindByIdAsync(string id)
		=> await _entity.FindAsync(id);

	public List<TEntity> GetAll()
		=> _entity.ToList();

	public async Task<List<TEntity>> GetAllAsync()
		=> await _entity.ToListAsync();

	public void Remove(TEntity entity)
		=> _entity.Remove(entity);

	public void Remove(string Id)
	{
		// var tEntity = new TEntity() { Id = Id };

		var tEntity = new TEntity();
		var idProperty = typeof(TEntity).GetProperty("Id");
		if (idProperty != null)
		{
			idProperty.SetValue(tEntity, Id, null);
		}

	}
	public void Update(TEntity entity)
		=> _entity.Update(entity);
}
