using Microsoft.Extensions.DependencyInjection;
using RatingService.Core.Interfaces;

namespace RatingService.Services.RatingService.Extensions;

public static class ProviderExtensions
{
    public static void AddRatingService(this IServiceCollection services)
    {
        services.AddScoped<IRatingService, RatingService>();
    }
}
