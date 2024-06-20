using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Models;

[PrimaryKey(nameof(Id))]
public class AchievementModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    [ForeignKey(nameof(OwnerUsername))]
    public string? OwnerUsername { get; set; }
    
    public string? Description { get; set; }
    public string? IconSVG { get; set; }
}