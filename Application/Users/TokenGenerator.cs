using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Users.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Application.Users;

public interface ITokenGenerator
{
    AccessToken Generate(Guid id);
}

public sealed class TokenGenerator(byte[] _key) : ITokenGenerator
{
    public AccessToken Generate(Guid id)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity
            (
                new List<Claim>
                {
                    new Claim("UserId", id.ToString()),
                }
            ),
            Expires = DateTime.UtcNow.AddYears(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessToken(tokenHandler.WriteToken(token));
    }
}