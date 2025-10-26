using Microsoft.EntityFrameworkCore;
using ReservationService.Database.Context.Configurations;
using ReservationService.Database.Models;

namespace ReservationService.Database.Context;

public class ReservationServiceContext : DbContext
{
    public DbSet<Reservation> Reservation { get; set; }
    
    public ReservationServiceContext(DbContextOptions<ReservationServiceContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
    }
}