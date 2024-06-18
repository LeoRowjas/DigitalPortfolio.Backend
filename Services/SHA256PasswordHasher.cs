using System.Security.Cryptography;
using DigitalPortfolio.API.Models;
using Microsoft.AspNetCore.Identity;

namespace DigitalPortfolio.API.Services;

public class Sha256PasswordHasher : IPasswordHasher<UserModel>
{
    private const int REHASH_COUNT = 10000;

    private const int HASH_SIZE_BYTES = 16;

    private const int SALT_SIZE_BYTES = 16;
    
    public string HashPassword(UserModel user, string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(SALT_SIZE_BYTES);

        using var deriveBytes = GetDeriveBytes(password, saltBytes);

        var hashBytes = deriveBytes.GetBytes(HASH_SIZE_BYTES);

        var fullHashBytes = new byte[SALT_SIZE_BYTES + HASH_SIZE_BYTES];

        Array.Copy(saltBytes, 0, fullHashBytes, 0, SALT_SIZE_BYTES);
        Array.Copy(hashBytes, 0, fullHashBytes, SALT_SIZE_BYTES, HASH_SIZE_BYTES);

        var hashedPassword = Convert.ToBase64String(fullHashBytes);

        return hashedPassword;
    }

    public PasswordVerificationResult VerifyHashedPassword(UserModel user, string hashedPassword, string providedPassword)
    {
        var fullHashBytes = Convert.FromBase64String(hashedPassword);

        var saltBytes = new byte[SALT_SIZE_BYTES];

        Array.Copy(fullHashBytes, 0, saltBytes, 0, SALT_SIZE_BYTES);

        using var deriveBytes = GetDeriveBytes(providedPassword, saltBytes);

        var hashBytes = deriveBytes.GetBytes(HASH_SIZE_BYTES);

        var isMatch = CompareHashes(hashBytes, fullHashBytes);

        return isMatch switch
        {
            true => PasswordVerificationResult.Success,
            false => PasswordVerificationResult.Failed
        };
    }
    
    private static bool CompareHashes(byte[] hashBytes, byte[] fullHashBytes)
    {
        for (var i = 0; i < HASH_SIZE_BYTES; i++)
        {
            if (hashBytes[i] != fullHashBytes[i + SALT_SIZE_BYTES])
            {
                return false;
            }
        }

        return true;
    }

    private static Rfc2898DeriveBytes GetDeriveBytes(string password, byte[] saltBytes)
    {
        return new Rfc2898DeriveBytes(password, saltBytes, REHASH_COUNT, HashAlgorithmName.SHA256);
    }
}