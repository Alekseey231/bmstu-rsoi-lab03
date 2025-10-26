using Microsoft.Extensions.Logging;
using RatingService.Core.Interfaces;
using RatingService.Core.Models;

namespace RatingService.Services.RatingService;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly ILogger<RatingService> _logger;

    public RatingService(IRatingRepository ratingRepository, ILogger<RatingService> logger)
    {
        _ratingRepository = ratingRepository;
        _logger = logger;
    }


    public async Task<Rating> GetRatingByUserNameAsync(string userName)
    {
        _logger.LogDebug("Get rating for user {UserName}", userName);
        
        var result = await _ratingRepository.GetRatingByUserNameAsync(userName);
        
        _logger.LogDebug("Got rating for user {UserName}", userName);
        
        return result;
    }

    public async Task<Rating> UpdateRatingAsync(string userName, int stars)
    {
        _logger.LogDebug("Update rating for user {UserName}", userName);
        
        if (stars is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(stars), "Stars must be between 0 and 100.");
        
        var result = await _ratingRepository.UpdateRatingAsync(userName, stars);
        
        _logger.LogDebug("Updated rating for user {UserName}", userName);
        
        return result;
    }
}