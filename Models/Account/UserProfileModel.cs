using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Models;

[PrimaryKey(nameof(Username))]
public class UserProfileModel
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Biography { get; set; }
    public List<Guid>? Projects { get; set; }
    public List<Guid>? Achievements { get; set; }
    public List<LikeModel>? Likes { get; set; }
    public string? Password { get; set; }
}