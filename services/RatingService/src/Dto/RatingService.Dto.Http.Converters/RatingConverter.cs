using CoreRating = RatingService.Core.Models.Rating;
using DtoRating = RatingService.Dto.Http.Models.Rating;

namespace RatingService.Dto.Http.Converters;

public static class RatingConverter
{
    public static DtoRating Convert(CoreRating model)
    {
        return new DtoRating(model.UserName,
            model.Stars);
    }
}