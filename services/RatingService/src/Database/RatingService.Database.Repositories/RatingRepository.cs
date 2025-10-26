using Microsoft.EntityFrameworkCore;
using RatingService.Core.Exceptions;
using RatingService.Core.Interfaces;
using RatingService.Core.Models;
using RatingService.Database.Context;
using RatingService.Database.Repositories.Converters;

namespace RatingService.Database.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly RatingServiceContext _context;

    public RatingRepository(RatingServiceContext context)
    {
        _context = context;
    }

    public async Task<Rating> GetRatingByUserNameAsync(string userName)
    {
        var rating = await _context.Rating.FirstOrDefaultAsync(r => r.UserName == userName);
        if (rating is null)
            throw new RatingNotFoundException();
        
        return RatingConverter.Convert(rating);
    }

    public async Task<Rating> UpdateRatingAsync(string userName, int stars)
    {
        var rating = await _context.Rating.FirstOrDefaultAsync(r => r.UserName == userName);
        if (rating is null)
            throw new RatingNotFoundException();
        
        rating.Stars = stars;
        
        await _context.SaveChangesAsync();
        
        return RatingConverter.Convert(rating);
    }
}