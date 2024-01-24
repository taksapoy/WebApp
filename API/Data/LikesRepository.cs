using api;
using API.DTOs;
using API.Entities;
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

    public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
    {
        var users = _dataContext.Users.OrderBy(user => user.UserName).AsQueryable();
        var likes = _dataContext.Likes.AsQueryable();

        if (predicate == "liked")
        {
            likes = likes.Where(like => like.SourceUserId == userId);
            users = likes.Select(like => like.LikedUser!);
        }
        if (predicate == "likedBy")
        {
            likes = likes.Where(like => like.LikedUserId == userId);
            users = likes.Select(like => like.SourceUser!);
        }

        return await users.Select(user => new LikeDto
        {
            UserName = user.UserName,
            Aka = user.Aka,
            City = user.City,
            Country = user.Country,
            Age = user.BirthDate.CalculateAge(),
            MainPhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain).Url,
            Id = user.Id
        }).ToListAsync();
    }
}
