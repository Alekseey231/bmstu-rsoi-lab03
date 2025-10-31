using RatingService.Dto.Http;
using RatingService.Dto.Http.Models;
using Refit;

namespace GatewayService.Clients;

public interface IRatingServiceClient
{
    [Get("/api/v1/rating")]
    Task<Rating> GetRatingAsync([Header("X-User-Name")] string userName);

    [Patch("/api/v1/rating")]
    Task<Rating> UpdateRatingAsync(
        [Header("X-User-Name")] string userName,
        [Body] UpdateRatingRequest request);
}