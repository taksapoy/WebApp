using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddJWTService(builder.Configuration);
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
try
{
    var dataContext = service.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    await Seed.SeedUsers(dataContext);
}
catch (System.Exception e)
{
    var log = service.GetRequiredService<ILogger<Program>>();
    log.LogError(e, "an error occurred during migration !!");
}

app.Run();

