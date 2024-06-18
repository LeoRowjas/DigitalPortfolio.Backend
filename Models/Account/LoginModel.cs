using System.ComponentModel.DataAnnotations;

namespace DigitalPortfolio.API.Models.Account;

public class LoginModel
{
    public required string Login { get; set; }
    
    [MinLength(5)]
    public required string Password { get; set; }
}