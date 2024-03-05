using Microsoft.EntityFrameworkCore;
using Music.Domain.Interface;
using Music.Infrastructure.Data.Context;

namespace Music.Infrastructure.Data.Repository;

public class MusicRepository : IMusicRepository
{
    private readonly MusicDBContext dbContext;

    public MusicRepository(MusicDBContext dbContext )
    {
        this.dbContext = dbContext;
    }
    public IEnumerable<Domain.Entities.Music> GetMusices()
    {
        return dbContext.Musices;
    }
}
