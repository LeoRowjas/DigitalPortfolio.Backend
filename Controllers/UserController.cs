using DigitalPortfolio.API.Data;
using DigitalPortfolio.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(DigitalPortfolioDbContext context) : ControllerBase
{
    private readonly DigitalPortfolioDbContext _context = context;
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null) return BadRequest("User not found");

        return Ok(user);
    }
    
    [HttpPost]
    [Authorize]
    [Route("add")]
    public async Task<IActionResult> PostUser(UserModel user)
    {
        var containsNullProperty = user.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string) || pi.PropertyType == typeof(Guid))
            .Select(pi => (string)pi.GetValue(user)!)
            .Any(string.IsNullOrEmpty);
        if(containsNullProperty) return BadRequest("Some required fields is null");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    [HttpPatch]
    [HttpPut]
    [Authorize]
    [Route("edit/{id:guid}")]
    public async Task<IActionResult> EditUser(Guid id, [FromBody] UserModel user)
    {
        var existingUser = await context.Users.FindAsync(id);
        if(existingUser == null) return BadRequest("User not found");
            
        existingUser.Username = user.Username;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Biography = user.Biography;
        existingUser.Email = user.Email;

        context.Users.Update(existingUser);
        await context.SaveChangesAsync();

        return Ok(existingUser);
    }

    [HttpDelete]
    [Authorize]
    [Route("delete/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if(user == null) return BadRequest("User not found");
        
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        
        return Ok(user);
    }
}