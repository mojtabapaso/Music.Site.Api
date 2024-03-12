using System.Collections.Generic;
using Music.Domain.Common;

namespace Music.Application.Interface;

public interface IGenericServices<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity entity);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void Remove(string Id);
    TEntity FindById(string id);
    Task<TEntity> FindByIdAsync(string id);
    List<TEntity> GetAll();
    Task<List<TEntity>> GetAllAsync();
}
