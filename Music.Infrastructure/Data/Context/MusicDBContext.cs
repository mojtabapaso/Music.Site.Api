using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;

namespace Music.Infrastructure.Data.Context;

public class MusicDBContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
{
	public MusicDBContext(DbContextOptions options) : base(options)
	{
	}
	public DbSet<ApplicationUser> Users { get; set; }
	public DbSet<MusicEntity> Musices { get; set; }
	public DbSet<Singer> Singeres { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}
