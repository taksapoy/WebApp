using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class LikesController : BaseApiController
{
    private readonly IlikesRepository _ilikesRepository;
    private readonly IUserRepository _userRepository;

    public LikesController(IlikesRepository ilikesRepository, IUserRepository userRepository)
    {
        _ilikesRepository = ilikesRepository;
        _userRepository = userRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username) 
    {
        var string_user_id = User.GetUserId();
        if (string_user_id is null) return NotFound();
        
         var sourceUserId = (int)string_user_id;
        // var likedUser = await _userRepository.GetUserByUserNameAsync(username);
        var likedUser = await _userRepository.GetUserByUserNameWithOutPhotoAsync(username);
        if (likedUser is null) return NotFound();

        var sourceUser = await _ilikesRepository.GetUser(sourceUserId);
        if (sourceUser.UserName == username) return BadRequest("can't like yourself");

        var userLike = await _ilikesRepository.GetUserLike(sourceUserId, likedUser.Id);
        if (userLike is not null) return BadRequest($"already like this user {likedUser.UserName}");

        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            LikedUserId = likedUser.Id
        };

        sourceUser.LikedUsers!.Add(userLike);
        if (await _userRepository.SaveAllAsync()) return Ok(); //not good, but work

        return BadRequest("Something has gone wrong!");
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
    {
        var string_user_id = User.GetUserId();
        if (string_user_id is null) return NotFound();

        var users = await _ilikesRepository.GetUserLikes(predicate, (int)string_user_id);
        return Ok(users);
    }
}