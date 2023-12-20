using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data ;


public class UserRepository : IUserRepository
{
   private IMapper _mapper ;
   private readonly DataContext _dataContext ;

   public UserRepository (IMapper mapper,DataContext dataContext)
    {
        _mapper = mapper ;
        _dataContext = dataContext ;
    }

    public async Task<MemberDto?> GetMemberByUserNameAsync(string username)
    {
        return await _dataContext.Users
            // .Include(user => user.Photos)
            .Where(user => user.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
       return await _dataContext.Users
            // .Include(user => user.Photos)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
       return await _dataContext.Users.FindAsync(id);
    }

    // public async Task<AppUser?> GetUserByUserNameAsync(string username)
    // {
    //     return await _dataContext.Users
    //         .Include(user => user.Photos)
    //         .SingleOrDefaultAsync(user => user.UserName == username);
    // }

    // public async Task<IEnumerable<AppUser>> GetUsersAsync()
    // {
    //     return await _dataContext.Users.Include(user => user.Photos).ToListAsync();
    // }

    public async Task<bool> SaveAllAsync()
    {
       return await _dataContext.SaveChangesAsync() > 0;
    }

    void IUserRepository.Update(AppUser user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
    }
}