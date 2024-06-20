namespace DigitalPortfolio.API.Services.Interfaces;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool VerifyHashedPassword(string password, string hashedPassword);
}