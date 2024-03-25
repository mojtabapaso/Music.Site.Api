using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;

namespace Music.Infrastructure.Context;

public class MusicDBContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public MusicDBContext()
    {
    }

    public MusicDBContext(DbContextOptions<MusicDBContext> options) : base(options)
    {
    }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<MusicEntity> Musics { get; set; }
    public DbSet<Singer> Singers { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<UserRefreshTokens> UserRefreshToken { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
