using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Entities;
using Warehouse.Token.Interfaces;

namespace Warehouse.Token;

public class TokenService : ITokenService
{
    public TokenService(IConfiguration configuration)
    {
        Console.WriteLine(configuration["TokenKey"]);
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"] ?? string.Empty));
    }

    private readonly SymmetricSecurityKey _securityKey;

    public string CreateToken(IdentityUser appUser)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, appUser.Id),
            new(JwtRegisteredClaimNames.Name, appUser.NormalizedUserName),
        };

        var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}