
using System.Security.Cryptography;
using System.Text;
using api.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api;

public class AccountController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly DataContext _dataContext;
  private readonly ITokenService _tokenService;

  public AccountController(IMapper mapper ,DataContext dataContext, ITokenService tokenService)
  {
    _mapper = mapper;
        _mapper = mapper;
        _dataContext = dataContext;
    _tokenService = tokenService;
  }

  [HttpPost("register")]
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (await isUserExists(registerDto.Username!))
      return BadRequest("Username is taken");

 
    using var hmacSHA256 = new HMACSHA256();
    var user = _mapper.Map<AppUser>(registerDto);

     // {
        user.UserName = registerDto.Username!.Trim().ToLower();
        user.PasswordHash = hmacSHA256.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!.Trim()));
        user.PasswordSalt = hmacSHA256.Key;
        // };
  
    _dataContext.Users.Add(user);
    await _dataContext.SaveChangesAsync();
    return new UserDto
    {
      Username = user.UserName,
      Token = _tokenService.CreateToken(user),
      Aka = user.Aka,
      Gender = user.Gender
    };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
  {
    var user = await _dataContext.Users
                        .Include(photo => photo.Photos)
                        .SingleOrDefaultAsync(user =>
                            user.UserName == loginDto.Username);

    if (user == null)
      return Unauthorized("Invalid username");


    using var hmac = new HMACSHA256(user.PasswordSalt);

    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    for (int i = 0; i < computedHash.Length; i++)
    {
      if (computedHash[i] != user.PasswordHash[i])
        return Unauthorized("Invalid password");
    }

    return new UserDto
    {
      Username = user.UserName,
      Token = _tokenService.CreateToken(user),
      PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
      Aka = user.Aka,
      Gender = user.Gender
    };
  }

  private async Task<bool> isUserExists(string username)
  {
    return await _dataContext.Users.AnyAsync(x => x.UserName == username.ToLower());
  }
}
