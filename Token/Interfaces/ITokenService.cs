using Microsoft.AspNetCore.Identity;

namespace Warehouse.Token.Interfaces;

public interface ITokenService
{
    string CreateToken(IdentityUser appUser);
}