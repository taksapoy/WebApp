using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    // Task<AppUser?> GetUserByUserNameAsync(string username);
    // Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto?> GetMemberByUserNameAsync(string username);
}