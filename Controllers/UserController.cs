using DigitalPortfolio.API.Data;
using DigitalPortfolio.API.Models.Account;
using DigitalPortfolio.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(DigitalPortfolioDbContext context, IAccountService accountService) : ControllerBase
{
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var unsecureUsers = await context.Users.ToListAsync();
        var users = unsecureUsers.Select(x => new
        {
            x.Username,
            x.FirstName,
            x.LastName,
            x.Biography,
            x.Email,
            x.Achievements,
            x.Projects,
        });
        return Ok(users);
    }

    [HttpGet]
    [Route("{username}")]
    public async Task<IActionResult> GetUserInfo(string username)
    {
        var user = await context.Users.FindAsync(username);
        if(user == null) return BadRequest("User not found");
        
        return Ok(new
        {
            user.Username,
            user.FirstName,
            user.LastName,
            user.Biography,
            user.Achievements,
            user.Projects,
        });
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel loginModel)
    {
        return await accountService.LoginAsync(loginModel);
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerModel)
    {
        return await accountService.RegisterAsync(registerModel);
    }

    [HttpPut]
    [Authorize]
    [Route("edit/{username}")]
    public async Task<IActionResult> EditUser(string username, [FromBody] SelfEditModel selfEditModel)
    {
        var user = await context.Users.FindAsync(username);
        if (user == null) return BadRequest("User not found");

        user.FirstName = selfEditModel.FirstName;
        user.LastName = selfEditModel.LastName;
        user.Biography = selfEditModel.Biography;
        user.Email = selfEditModel.Email;
        await context.SaveChangesAsync();
        
        return Ok("User updated successfully");
    }

    [HttpDelete]
    [Authorize]
    [Route("delete/{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var user = await context.Users.FindAsync(username);
        if (user == null) return BadRequest("User not found");

        try
        {
            context.Projects.RemoveRange(user.Projects?.Select(x => context.Projects.Find(x))!);
            context.Likes.RemoveRange(user.Likes?.Select(x => context.Likes.Find(x))!);
            context.Achievement.RemoveRange(user.Achievements?.Select(x => context.Achievement.Find(x))!);
        }
        catch (Exception e)
        {// ignored
        }
        
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        
        return Ok("User deleted successfully");
    }
}