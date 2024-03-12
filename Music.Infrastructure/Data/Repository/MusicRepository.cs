using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;
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
    public IEnumerable<MusicEntity> GetMusices()
    {
        return dbContext.Musices;
    }
}
