using API.Extensions;
using api.Middleware;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
.WithOrigins("https://localhost:4200"));
app.UseMiddleware<ExceptionMiddleware>();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;

try
{
  var dataContext = service.GetRequiredService<DataContext>();
  var userManager = service.GetRequiredService<UserManager<AppUser>>();
  var roleManager = service.GetRequiredService<RoleManager<AppRole>>();
  await dataContext.Database.MigrateAsync();
  await Seed.SeedUsers(userManager, roleManager);
  
  await dataContext.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]"); //<--good for sqlite
}
catch (Exception e)
{
  var log = service.GetRequiredService<ILogger<Program>>();
  log.LogError(e, "an error occurred during migration !!");
}


app.Run();
