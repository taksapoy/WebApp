﻿using api;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class AppServiceExtensions
{
  public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration conf)
  {
    services.AddDbContext<DataContext>(opt =>
    {
      // opt.UseSqlite(conf.GetConnectionString("SqliteConnection"));
       opt.UseNpgsql(conf.GetConnectionString("DefaultConnection"));
    });
    services.AddCors();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IImageService, ImageService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IMessageRepository, MessageRepository>();
    services.AddScoped<LogUserActivity>();
    services.AddScoped<IlikesRepository, LikesRepository>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.Configure<CloudinarySettings>(conf.GetSection("CloudinarySettings"));
    services.AddSignalR();
    services.AddSingleton<PresenceTracker>();
    
    return services;
  }
}