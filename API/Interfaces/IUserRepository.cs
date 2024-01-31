using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
  void Update(AppUser user);
  Task<bool> SaveAllAsync();
  Task<AppUser> GetUserByIdAsync(int id);
  Task<AppUser> GetUserByUserNameAsync(string username);
  Task<IEnumerable<MemberDto>> GetUsersAsync();
  // Task<IEnumerable<MemberDto>> GetMembersAsync();
   Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
  Task<MemberDto> GetMemberAsync(string username);
    Task<AppUser> GetUserByUserNameWithOutPhotoAsync(string username);

    // Task<MemberDto> GetMemberByUserNameAsync(string username);
}