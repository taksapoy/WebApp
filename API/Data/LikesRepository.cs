using api;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository : IlikesRepository
{
    private readonly DataContext _dataContext;

    public LikesRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<AppUser> GetUser(int userId)
    {
        return await _dataContext.Users.Include(user => user.LikedUsers)
              .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        return await _dataContext.Likes.FindAsync(sourceUserId, likedUserId);
    }

    public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var users = _dataContext.Users.OrderBy(user => user.UserName).AsQueryable();
        var likes = _dataContext.Likes.AsQueryable();

        if (likesParams.Predicate == "liked")
        {
            likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
            users = likes.Select(like => like.LikedUser!);
        }
        if (likesParams.Predicate == "likedBy")
        {
            likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
            users = likes.Select(like => like.SourceUser!);
        }

        var likedUsers= users.Select(user => new LikeDto
        {
            UserName = user.UserName,
            Aka = user.Aka,
            City = user.City,
            Country = user.Country,
            Age = user.BirthDate.CalculateAge(),
            MainPhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain).Url,
            Id = user.Id
        }); //.ToListAsync();
        return await PageList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
    }
}
