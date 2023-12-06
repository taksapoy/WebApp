using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class AppServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration conf)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(conf.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}