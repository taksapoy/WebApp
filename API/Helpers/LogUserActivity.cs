using api;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        var user = resultContext.HttpContext.User;
        if (user is null) return;
        if (user.Identity is not null && !user.Identity.IsAuthenticated) return;

         var userID = user.GetUserId(); //user.GetUsername();
        if (userID is null) return;

         var repository = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var userRepository = await repository.GetUserByIdAsync((int)userID);
        if (userRepository is null) return;

        userRepository.LastActive = DateTime.UtcNow;
        await repository.SaveAllAsync();
    }
}