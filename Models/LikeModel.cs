using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalPortfolio.API.Models;

public class LikeModel
{
    public Guid Id { get; set; }
    [ForeignKey("OwnerId")]
    public Guid OwnerId { get; set; }
    public Guid ProjectId { get; set; }
}