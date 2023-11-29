using API. Entities;

namespace API.interfaces ;

public interface ITokenService
{
    string CreateToken(AppUser user) ;
}
