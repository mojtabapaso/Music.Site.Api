using Microsoft.EntityFrameworkCore;
using Music.Infrastructure.Data.Context;
using Music.Infrastructure.IocConfig;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;

Services.AddDbContext<MusicDBContext>(option =>
{
    var ee = builder.Configuration.GetConnectionString("MusicDBConecnection");
    option.UseSqlite(ee);

    });
Services.AddControllers();
Services.AddEndpointsApiExplorer();
Services.AddSwaggerGen();
Services.AddResponseCaching();
Services.AddMemoryCache();
Services.AddEntityServies();
Services.AddIdentityServies();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();
app.UseCors("EnableCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//app.UseApiVersioning();

app.Run();
