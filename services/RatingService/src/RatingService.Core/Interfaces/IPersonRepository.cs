using RatingService.Core.Models;

namespace RatingService.Core.Interfaces;

public interface IRatingRepository
{
    public Task<Rating> GetRatingByUserNameAsync(string userName);
    
    public Task<Rating> UpdateRatingAsync(string userName, int stars);
}