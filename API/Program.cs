using API.Extensions;
using api.Middleware;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using API.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;

try
{
  var dataContext = service.GetRequiredService<DataContext>();
  var userManager = service.GetRequiredService<UserManager<AppUser>>();
  await dataContext.Database.MigrateAsync();
  await Seed.SeedUsers(userManager);
}
catch (Exception e)
{
  var log = service.GetRequiredService<ILogger<Program>>();
  log.LogError(e, "an error occurred during migration !!");
}


app.Run();