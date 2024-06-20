using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Models;

[PrimaryKey(nameof(ProjectId), nameof(OwnerUsername))]
public class LikeModel
{
    public Guid ProjectId { get; set; }
    
    [ForeignKey(nameof(OwnerUsername))]
    public string? OwnerUsername { get; set; }
}