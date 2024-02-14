using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration conf)
  {
    services.AddIdentityCore<AppUser>(opts =>
        {
            opts.Password.RequiredLength = 8;
            // opts.User.RequireUniqueEmail = false;
        })
        .AddRoles<AppRole>()
        .AddRoleManager<RoleManager<AppRole>>()
        .AddEntityFrameworkStores<DataContext>();

        
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
      opts.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(conf["TokenKey"]!)
              ),
        ValidateIssuer = false,
        ValidateAudience = false,
      };
      opts.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
    });


     services.AddAuthorization(opts =>
        {
            opts.AddPolicy("AdminRole", policy => policy.RequireRole("Administrator"));
            opts.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Administrator", "Moderator"));
        });
    return services;
  }
}