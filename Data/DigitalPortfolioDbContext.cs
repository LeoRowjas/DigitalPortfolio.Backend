using DigitalPortfolio.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalPortfolio.API.Data;

public class DigitalPortfolioDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<ProjectModel> Projects { get; set; }
    public DbSet<AchievementModel> Achievement { get; set; }
    public DbSet<LikeModel> Likes { get; set; }
}