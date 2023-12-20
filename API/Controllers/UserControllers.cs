using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


//[Authorize]
public class UsersController : BaseApiController
{
    private readonly UserRepository userRepository;
    private IUserRepository _userRepository;
    private IMapper _mapper;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        // var users = await _userRepository.GetUsersAsync();
        // return Ok(_mapper.Map<IEnumerable<MemberDto>>(users));
        return Ok (await _userRepository.GetMembersAsync());
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto?>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<MemberDto>(user);
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<MemberDto?>> GetUserByUserName(string username)
    {
        return await _userRepository.GetMemberByUserNameAsync(username);
    }

}

