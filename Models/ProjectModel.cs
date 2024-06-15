using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Models;

public class ProjectModel
{
    public Guid Id { get; set; }
    [ForeignKey("OwnerId")]
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int LikesCount { get; set; }
    public DateTime ReceiveDate { get; set; }
}