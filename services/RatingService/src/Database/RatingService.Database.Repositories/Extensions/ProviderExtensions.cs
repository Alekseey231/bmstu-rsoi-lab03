using Microsoft.Extensions.DependencyInjection;
using RatingService.Core.Interfaces;

namespace RatingService.Database.Repositories.Extensions;

public static class ProviderExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRatingRepository, RatingRepository>();
    }
}