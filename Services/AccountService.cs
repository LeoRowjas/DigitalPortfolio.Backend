using DigitalPortfolio.API.Data;
using DigitalPortfolio.API.Helpers;
using DigitalPortfolio.API.Models;
using DigitalPortfolio.API.Models.Account;
using DigitalPortfolio.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalPortfolio.API.Services;

public class AccountService(DigitalPortfolioDbContext context, IPasswordHasher passwordHasher) : IAccountService
{
    public async Task<IActionResult> LoginAsync(LoginModel loginModel)
    {
        var user = await context.Users.FindAsync(loginModel.Login);
        
        if (user is null)
        {
            return new BadRequestObjectResult(new { Field = nameof(loginModel.Login) });
        }

        var verified = passwordHasher.VerifyHashedPassword(loginModel.Password, user.Password!);

        if (!verified)
        {
            return new BadRequestObjectResult(new { Field = nameof(loginModel.Password) });
        }

        var token = SecurityHelper.GetAuthorizationToken(user);

        return new OkObjectResult(new
        {
            user.Username,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Biography,
            token
        });
    }
    
    public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
    {
        var isLoginExist = await context.Users.FindAsync(registerModel.Login) is not null;

        if (isLoginExist)
        {
            return new ConflictObjectResult(new { Field = nameof(registerModel.Login) });
        }

        var user = CreateUser(registerModel, passwordHasher);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var token = SecurityHelper.GetAuthorizationToken(user);

        return new OkObjectResult(new
        {
            user.Username,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Biography,
            token
        });
    }
    
    public async Task<IActionResult> GetUserInfoAsync(string username)
    {
        var user = await context.Users.FindAsync();
        
        if (user is null)
        {
            return new BadRequestObjectResult($"No user with such username: {username}");
        }

        return new OkObjectResult(new
        {
            user.Username,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Biography,
        });
    }
    
    public async Task<IActionResult> SelfEditAsync(SelfEditModel selfEditModel, string username)
    {
        var user = await context.Users.FindAsync(username);

        if (user is null)
        {
            return new BadRequestResult();
        }
        
        user.FirstName = selfEditModel.FirstName ?? user.FirstName;
        user.LastName = selfEditModel.LastName ?? user.LastName;
        user.Email = selfEditModel.Email ?? user.Email;
        user.Biography = selfEditModel.Biography ?? user.Biography;

        await context.SaveChangesAsync();

        return new OkResult();
    }
    
    private static UserProfileModel CreateUser(RegisterModel registerModel, IPasswordHasher hasher)
    {
        var hashedPassword = hasher.HashPassword(registerModel.Password);

        return new UserProfileModel() 
        {
            Username = registerModel.Login,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Email = registerModel.Email,
            Password = hashedPassword,
            Biography = registerModel.Biography,
        };
    }
}