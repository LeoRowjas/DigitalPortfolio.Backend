using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DigitalPortfolio.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace DigitalPortfolio.API.Helpers;

public class SecurityHelper
{
    public static string GetAuthorizationToken(UserModel user)
    {
        var userId = user.Id.ToString();
        var email = user.Email;

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email)
        };

        var jwt = new JwtSecurityToken(
            claims: claims,
            issuer: AuthorizationOptions.TOKEN_ISSUER,
            audience: AuthorizationOptions.TOKEN_AUDIENCE,
            expires: DateTime.UtcNow + AuthorizationOptions.AuthTokenLifetime,
            signingCredentials: new SigningCredentials(
                AuthorizationOptions.SymmetricSecurityKey,
                SecurityAlgorithms.HmacSha256)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }
}