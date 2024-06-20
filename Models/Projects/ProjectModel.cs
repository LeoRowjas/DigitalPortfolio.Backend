using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Models;

[PrimaryKey(nameof(ProjectId))]
public class ProjectModel
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    
    [ForeignKey(nameof(OwnerName))]
    public string OwnerName { get; set; }
    
    public string Description { get; set; }
    public int LikesCount { get; set; }
    public DateTime CreationDateTime { get; set; }
}