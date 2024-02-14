using api.Controllers;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    public AdminController (UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [Authorize(Policy = "AdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        if (string.IsNullOrEmpty(roles)) return BadRequest("must at least one role to assign");
        var newUserRoles = roles.Split(",").ToArray();

        var user = await _userManager.FindByNameAsync(username);
        if (user is null) return NotFound(username);
        var curentUserRoles = await _userManager.GetRolesAsync(user);

        var setRolesResult = await _userManager.AddToRolesAsync(user, newUserRoles.Except(curentUserRoles));
        if (!setRolesResult.Succeeded) return BadRequest(setRolesResult.Errors);

        setRolesResult = await _userManager.RemoveFromRolesAsync(user, curentUserRoles.Except(newUserRoles));
        if (!setRolesResult.Succeeded) return BadRequest(setRolesResult.Errors);

        return Ok(await _userManager.GetRolesAsync(user));
    }

    
    [Authorize(Policy = "AdminRole")]
    [HttpGet("users-with-roles")]

     public async Task<ActionResult> GetUsersRoles() {
        var users = await _userManager.Users
                    .OrderBy(appUser => appUser.UserName)
                    .Select(appUser => new
                    {
                        appUser.Id,
                        Username = appUser.UserName,
                        Roles = appUser.UserRoles
                                .Select(appUserRole => appUserRole.Role.Name).ToList()
                    })
                    .ToListAsync();
        return Ok(users);
    }


    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]


    public ActionResult GetPhotosForModeration() {
        return Ok("Hello Admin/Moderator");
    }
}