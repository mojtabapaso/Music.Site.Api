using Music.Domain.Entities;

namespace Music.Application.Interface.Entity;

public interface IMusicServices : IGenericServices<MusicEntity>
{
    public Task<(List<MusicEntity> musicEntities, int totalCount, int totalPages)> PaginationMusicAsync(string filter, int page = 1, int pageSize = 10);
    public Task<List<MusicEntity>> GetSubscriptionRequiredMusicAsync(int pageNumber, int pageSize);
    public Task<List<MusicEntity>> GetSubscriptionNotRequiredMusicAsync(int pageNumber, int pageSize);
    public Task<MusicEntity> FindSubscriptionNotRequiredMusicAsync(string id);
    public Task<MusicEntity> FindSubscriptionRequiredMusicAsync(string id);
    public Task<List<MusicEntity>> GetMusicListBySingerAsync(string singerId);
    public Task<List<MusicEntity>> GetMusicListByCategoryAsync(string categoryName);
}
