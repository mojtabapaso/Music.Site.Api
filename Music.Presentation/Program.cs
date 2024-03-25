using Music.Infrastructure.IocConfig;

var builder = WebApplication.CreateBuilder(args);
var Services = builder.Services;
// Local Service
Services.AddControllers();
Services.AddEndpointsApiExplorer();
Services.AddSwaggerGen();
Services.AddRepositoryServies();
Services.AddResponseCaching();
Services.AddMemoryCache();
// Custome Service
Services.AddDbContextServies();
Services.AddIdentityServies();
Services.AddHangfireServies();

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
// Custome App
app.AddHangfireApp();
//app.UseApiVersioning();
app.Run();
public partial class Program { }
