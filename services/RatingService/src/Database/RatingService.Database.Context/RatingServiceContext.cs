using Microsoft.EntityFrameworkCore;
using RatingService.Database.Models;
using RatingService.Database.Context.Configurations;

namespace RatingService.Database.Context;

public class RatingServiceContext : DbContext
{
    public DbSet<Rating> Rating { get; set; }
    
    public RatingServiceContext(DbContextOptions<RatingServiceContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
    }
}