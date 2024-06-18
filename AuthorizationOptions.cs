using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DigitalPortfolio.API;

public class AuthorizationOptions
{
    public const string TOKEN_ISSUER = "Default";

    public const string TOKEN_AUDIENCE = "Default";

    public static readonly TimeSpan AuthTokenLifetime = TimeSpan.FromDays(10);

    public static string? SecurityKey { get; private set; }

    public static SymmetricSecurityKey SymmetricSecurityKey
    {
        get
        {
            ArgumentNullException.ThrowIfNull(SecurityKey);

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
        }
    }

    public static void Initialize(string securityKey)
    {
        ArgumentNullException.ThrowIfNull(securityKey);

        SecurityKey = securityKey;
    }
}