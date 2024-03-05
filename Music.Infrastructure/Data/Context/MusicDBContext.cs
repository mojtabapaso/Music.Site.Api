using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;

namespace Music.Infrastructure.Data.Context;

public class MusicDBContext : DbContext
{

    public MusicDBContext(DbContextOptions<MusicDBContext> options)
    : base(options)
    {

    }

    public DbSet<Domain.Entities.Music> Musices { get; set; }
    public DbSet<Singer> Singeres { get; set; }


}
