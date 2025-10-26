using RatingService.Dto.Http.Models;

namespace GatewayService.Dto.Http.Converters;

public static class RatingConverter
{
    public static UserRatingResponse Convert(Rating rating)
    {
        return new UserRatingResponse(rating.Stars);
    }
}