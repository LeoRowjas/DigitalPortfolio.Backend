using System.Security.Claims;
using DigitalPortfolio.API.Data;
using DigitalPortfolio.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(DigitalPortfolioDbContext context) : ControllerBase
{
    [HttpGet]
    [Route("userall/{username}")]
    public async Task<IActionResult> GetAllUserProject(string username)
    {
        var user = await context.Users.FindAsync(username);
        if (user is null) return BadRequest("User not found");
        
        var projectsId = user.Projects;
        var projects = projectsId?.Select(x => context.Projects.Find(x)).ToList();
        
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
    [Route("add")]
    public async Task<IActionResult> CreateProject([FromBody]ProjectModel project)
    {
        var containsNullProperty = project.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => (string)pi.GetValue(project)!)
            .Any(string.IsNullOrEmpty);

        if (containsNullProperty) return BadRequest("Model is not valid");
        
        var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await context.Users.FindAsync(username);
        
        var isFirstProject = user?.Projects is null;
        if (isFirstProject)
        {
            var achievement = new AchievementModel()
            {
                Id = Guid.NewGuid(),
                Title = "The Way",
                OwnerUsername = username,
                Description = "Create first project!",
                IconSVG = "<svg fill=\"#000000\" viewBox=\"0 0 32 32\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\" version=\"1.1\" xml:space=\"preserve\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:serif=\"http://www.serif.com/\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"><g id=\"SVGRepo_bgCarrier\" stroke-width=\"0\"></g><g id=\"SVGRepo_tracerCarrier\" stroke-linecap=\"round\" stroke-linejoin=\"round\"></g><g id=\"SVGRepo_iconCarrier\"> <g transform=\"matrix(1,0,0,1,-96,-192)\"> <g transform=\"matrix(0.785714,0,0,0.785714,-15.2857,40)\"> <path d=\"M162,196L165.152,198.235L168.65,198.482L170.613,201.387L174.124,203L173.765,206.848L175.3,210L173.765,213.152L174.124,217L170.613,218.613L168.65,221.518L165.152,221.765L162,224L158.848,221.765L155.35,221.518L153.387,218.613L149.876,217L150.235,213.152L148.7,210L150.235,206.848L149.876,203L153.387,201.387L155.35,198.482L158.848,198.235L162,196Z\" style=\"fill:#90e0ef;\"></path> </g> <g transform=\"matrix(0.866025,0.5,-0.5,0.866025,121.962,-23.4275)\"> <path d=\"M102,213L102,221L105,219L108,221L108,213\" style=\"fill:#90e0ef;\"></path> </g> <g transform=\"matrix(-0.866025,0.5,0.5,0.866025,102.031,-23.4275)\"> <path d=\"M102,213L102,221L105,219L108,221L108,213\" style=\"fill:#90e0ef;\"></path> </g> <path d=\"M102.835,211.702L98.931,218.464C98.746,218.783 98.753,219.178 98.947,219.492C99.142,219.805 99.493,219.986 99.861,219.962L102.794,219.773C102.794,219.773 104.097,222.407 104.097,222.407C104.26,222.738 104.592,222.952 104.961,222.964C105.329,222.975 105.675,222.783 105.859,222.464L109.789,215.658L110.265,215.996C111.304,216.733 112.696,216.733 113.735,215.996L114.206,215.661L118.134,222.464C118.318,222.783 118.664,222.975 119.032,222.964C119.401,222.952 119.733,222.738 119.896,222.407L121.199,219.773C121.199,219.773 124.132,219.962 124.132,219.962C124.5,219.986 124.851,219.805 125.046,219.492C125.24,219.178 125.247,218.783 125.062,218.464L121.16,211.706C122.01,211.093 122.49,210.066 122.39,208.995C122.355,208.622 122.32,208.24 122.292,207.942C122.275,207.759 122.308,207.576 122.388,207.411C122.388,207.411 122.923,206.313 122.923,206.313C123.326,205.484 123.326,204.516 122.923,203.687C122.923,203.687 122.388,202.589 122.388,202.589C122.308,202.424 122.275,202.241 122.292,202.058C122.32,201.76 122.355,201.378 122.39,201.005C122.509,199.736 121.813,198.531 120.655,198C120.314,197.843 119.966,197.683 119.693,197.558C119.527,197.482 119.385,197.361 119.282,197.209C119.282,197.209 118.599,196.197 118.599,196.197C118.083,195.433 117.244,194.949 116.324,194.884C116.324,194.884 115.106,194.798 115.106,194.798C114.923,194.785 114.748,194.722 114.598,194.616C114.354,194.443 114.041,194.221 113.735,194.004C112.696,193.267 111.304,193.267 110.265,194.004L110.265,194.004C109.959,194.221 109.646,194.443 109.402,194.616C109.252,194.722 109.077,194.785 108.894,194.798C108.894,194.798 107.676,194.884 107.676,194.884C106.756,194.949 105.917,195.433 105.401,196.197C105.401,196.197 104.718,197.209 104.718,197.209C104.615,197.361 104.473,197.482 104.307,197.558C104.034,197.683 103.686,197.843 103.345,198C102.187,198.531 101.491,199.736 101.61,201.005C101.645,201.378 101.68,201.76 101.708,202.058C101.725,202.241 101.692,202.424 101.612,202.589C101.612,202.589 101.077,203.687 101.077,203.687C100.674,204.516 100.674,205.484 101.077,206.313C101.077,206.313 101.612,207.411 101.612,207.411C101.692,207.576 101.725,207.759 101.708,207.942C101.68,208.24 101.645,208.622 101.61,208.995C101.51,210.064 101.989,211.088 102.835,211.702ZM104.599,212.646L101.597,217.846L103.331,217.734C103.733,217.708 104.112,217.927 104.291,218.289C104.291,218.289 105.061,219.846 105.061,219.846L107.787,215.124L107.676,215.116C106.756,215.051 105.917,214.567 105.401,213.803C105.401,213.803 104.718,212.791 104.718,212.791C104.683,212.739 104.643,212.691 104.599,212.646ZM119.397,212.651C119.354,212.694 119.316,212.741 119.282,212.791C119.282,212.791 118.599,213.803 118.599,213.803C118.083,214.567 117.244,215.051 116.324,215.116L116.206,215.124L118.932,219.846L119.702,218.289C119.881,217.927 120.26,217.708 120.662,217.734C120.662,217.734 122.396,217.846 122.396,217.846L119.397,212.651ZM110.559,196.248L111.422,195.636C111.768,195.39 112.232,195.39 112.578,195.636L113.441,196.248C113.89,196.566 114.417,196.754 114.965,196.793L116.183,196.879C116.49,196.901 116.769,197.062 116.941,197.317L117.625,198.328C117.932,198.784 118.359,199.146 118.859,199.376L119.82,199.817C120.206,199.994 120.438,200.396 120.399,200.819L120.3,201.872C120.249,202.42 120.349,202.97 120.59,203.465L121.125,204.562C121.259,204.839 121.259,205.161 121.125,205.438L120.59,206.535C120.349,207.03 120.249,207.58 120.3,208.128L120.399,209.181C120.438,209.604 120.206,210.006 119.82,210.183L118.859,210.624C118.359,210.854 117.932,211.216 117.625,211.672L116.941,212.683C116.769,212.938 116.49,213.099 116.183,213.121L114.965,213.207C114.417,213.246 113.89,213.434 113.441,213.752L112.578,214.364C112.232,214.61 111.768,214.61 111.422,214.364L110.559,213.752C110.11,213.434 109.583,213.246 109.035,213.207L107.817,213.121C107.51,213.099 107.231,212.938 107.059,212.683L106.375,211.672C106.068,211.216 105.641,210.854 105.141,210.624L104.18,210.183C103.794,210.006 103.562,209.604 103.601,209.181L103.7,208.128C103.751,207.58 103.651,207.03 103.41,206.535L102.875,205.438C102.741,205.161 102.741,204.839 102.875,204.562L103.41,203.465C103.651,202.97 103.751,202.42 103.7,201.872L103.601,200.819C103.562,200.396 103.794,199.994 104.18,199.817L105.141,199.376C105.641,199.146 106.068,198.784 106.375,198.328L107.059,197.317C107.231,197.062 107.51,196.901 107.817,196.879L109.035,196.793C109.583,196.754 110.11,196.566 110.559,196.248ZM112,199C108.689,199 106,201.689 106,205C106,208.311 108.689,211 112,211C115.311,211 118,208.311 118,205C118,201.689 115.311,199 112,199ZM112,201C114.208,201 116,202.792 116,205C116,207.208 114.208,209 112,209C109.792,209 108,207.208 108,205C108,202.792 109.792,201 112,201Z\" style=\"fill:#1990a7;\"></path> </g> </g></svg>"
            };

            user.Achievements = new List<Guid> { achievement.Id };
        }

        user.Projects = new List<Guid> {project.ProjectId};
        
        context.Users.Update(user);
        
        context.Projects.Add(project);
        
        await context.SaveChangesAsync();

        return Ok(project.ProjectId);
    }
    
    [HttpPut]
    [Authorize]
    [Route("edit/{id:guid}")]
    public async Task<IActionResult> EditProject(Guid id, [FromBody]EditProjectModel editedProject)
    {
        var existingProject = await context.Projects.FindAsync(id);
        if (existingProject == null) return BadRequest("Project not found");

        existingProject.Title = editedProject.Title;
        existingProject.Description = editedProject.Description ?? "";

        context.Projects.Update(existingProject);
        await context.SaveChangesAsync();

        return Ok(editedProject);
    }
    
    [HttpDelete]
    [Authorize]
    [Route("delete/{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null) return BadRequest("Project not found");

        var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await context.Users.FindAsync(username);
        
        user?.Projects?.Remove(projectId);
        context.Users.Update(user!);

        var likes = context.Likes.Where(x => x.ProjectId == projectId).ToList();
        context.Likes.RemoveRange(likes);
        
        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return Ok(project.ProjectId);
    }
    
    [HttpPost]
    [Authorize]
    [Route("{projectId:guid}/like")]
    public async Task<IActionResult> LikeProject(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null) return BadRequest("Project not found");
        
        var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await context.Users.FindAsync(username);

        var like = new LikeModel()
        {
            OwnerUsername = username,
            ProjectId = projectId
        };
        
        var existingLike = await context.Likes.FindAsync(like.ProjectId, like.OwnerUsername);
        
        if(existingLike != null) return BadRequest("You already liked this project");

        user!.Likes = new List<LikeModel>() { like };
        user?.Likes!.Add(like);
        project.LikesCount++;

        context.Users.Update(user!);
        context.Projects.Update(project);
        context.Likes.Add(like);
        
        await context.SaveChangesAsync();

        return Ok(project.LikesCount);
    }
    
    [HttpDelete]
    [Authorize]
    [Route("{projectId:guid}/unlike")]
    public async Task<IActionResult> UnlikeProject(Guid projectId)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null) return BadRequest("Project not found");
        
        var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await context.Users.FindAsync(username);
        
        var like = new LikeModel()
        {
            OwnerUsername = username,
            ProjectId = projectId
        };
        
        var existingLike = await context.Likes.FindAsync(like.ProjectId, like.OwnerUsername);
        
        if(existingLike == null) return BadRequest("You didn't like this project");
        
        user?.Likes!.Remove(existingLike);
        context.Users.Update(user!);
        
        context.Likes.Remove(existingLike);
        
        project.LikesCount--;
        context.Projects.Update(project);
        
        await context.SaveChangesAsync();

        return Ok(project.LikesCount);
    }
}