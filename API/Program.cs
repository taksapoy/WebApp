using System.Text;
using API.Data;
using API.Extensions;
using API.interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddJWTService(builder.Configuration);

builder.Services.AddScoped<ITokenService,TokenService>();
var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));


app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.Run();