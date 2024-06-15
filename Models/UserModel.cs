using System.Text.Json.Serialization;

namespace DigitalPortfolio.API.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Biography { get; set; }
    public List<ProjectModel> Projects { get; set; }
    public List<AchievementModel> Achievements { get; set; }
    public List<LikeModel> Likes { get; set; }
    
    [JsonIgnore]
    public string Password { get; set; }
}