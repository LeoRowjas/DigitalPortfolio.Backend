using DigitalPortfolio.API.Data;
using DigitalPortfolio.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(DigitalPortfolioDbContext context) : ControllerBase
{
    [HttpGet]
    [Route("userall/{userId:guid}")]
    public async Task<IActionResult> GetAllUserProject(Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null) return BadRequest("User ot found");
        var projects = user.Projects;

        return Ok(projects);
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await context.Projects.ToListAsync();
        return Ok(projects);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var project = await context.Projects.FindAsync(id);
        if(project == null) return BadRequest("Project not found");

        return Ok(project);
    }
    
    [HttpPost]
    [Authorize]
    [Route("add/user={userId:guid}&project{projectId:guid}")]
    public async Task<IActionResult> CreateProject(Guid userId, Guid projectId, [FromBody]ProjectModel project)
    {
        var containsNullProperty = project.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => (string)pi.GetValue(project)!)
            .Any(string.IsNullOrEmpty);

        if (containsNullProperty) return BadRequest("Some required fields is null");
        
        var user = await context.Users.FindAsync(userId);
        if (user == null) return BadRequest("User not found");
        
        context.Users.FindAsync(userId).Result?.Projects.Add(project);
        
        await context.Projects.AddAsync(project);
        await context.SaveChangesAsync();

        return Ok(project);
    }
    
    [HttpPut]
    [HttpPatch]
    [Authorize]
    [Route("edit/{id:guid}")]
    public async Task<IActionResult> EditProject(Guid id, ProjectModel project)
    {
        var existingProject = await context.Projects.FindAsync(id);
        if (existingProject == null) return BadRequest("Project not found");

        context.Projects.Update(project);
        await context.SaveChangesAsync();

        return Ok(project);
    }
    
    [HttpDelete]
    [Authorize]
    [Route("delete/{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null) return BadRequest("Project not found");

        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return Ok(project);
    }
    
    [HttpPost]
    [Route("{projectId:guid}/like")]
    public async Task<IActionResult> LikeProject(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null) return BadRequest("Project not found");

        project.LikesCount++;
        await context.SaveChangesAsync();

        return Ok(project);
    }
    
    [HttpDelete]
    [Route("{projectId:guid}/unlike")]
    public async Task<IActionResult> UnlikeProject(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null) return BadRequest("Project not found");

        project.LikesCount--;
        await context.SaveChangesAsync();

        return Ok(project);
    }
}