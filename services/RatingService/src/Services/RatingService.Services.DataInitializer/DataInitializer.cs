using RatingService.Core.Interfaces;
using RatingService.Database.Context;
using RatingService.Database.Models;

namespace RatingService.Services.DataInitializer;

public class DataInitializer : IDataInitializer
{
    private readonly RatingServiceContext _context;
    
    public DataInitializer(RatingServiceContext context)
    {
        _context = context;
    }
    
    public async Task InitializeAsync()
    {
        var rating = new Rating("Test Max", 75);
        
        await _context.Rating.AddAsync(rating);
        await _context.SaveChangesAsync();
    }
 }