using System.ComponentModel.DataAnnotations;

namespace DigitalPortfolio.API.Models.Account;

public class RegisterModel
{
    public required string Login { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; } 
    
    [MinLength(5)]
    public required string Password { get; set; }
    
    public string? Biography { get; set; }
}