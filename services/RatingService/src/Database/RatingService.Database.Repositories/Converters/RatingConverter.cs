using DbRating = RatingService.Database.Models.Rating;
using CoreRating = RatingService.Core.Models.Rating;

namespace RatingService.Database.Repositories.Converters;

public static class RatingConverter
{
    public static CoreRating Convert(DbRating model)
    {
        return new CoreRating(model.UserName,
            model.Stars);
    }
}