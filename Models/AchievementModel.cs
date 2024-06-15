using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Models;

public class AchievementModel
{
    public Guid Id { get; set; }
    [ForeignKey("ProjectId")]
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string IconSVG { get; set; }
}