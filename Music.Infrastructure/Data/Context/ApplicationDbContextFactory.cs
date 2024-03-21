using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Music.Infrastructure.Data.Context;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<MusicDBContext>
{
    public ApplicationDbContextFactory()
    {
    }
    public MusicDBContext CreateDbContext(string[] args)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
        IConfigurationRoot root = builder.Build();
        string? sqlConnectionString = root["ConnectionStrings:MusicDBConecnection"];
        var optionsBuilder = new DbContextOptionsBuilder<MusicDBContext>();
        optionsBuilder.UseSqlServer(sqlConnectionString);
        return new MusicDBContext(optionsBuilder.Options);
    }
}
