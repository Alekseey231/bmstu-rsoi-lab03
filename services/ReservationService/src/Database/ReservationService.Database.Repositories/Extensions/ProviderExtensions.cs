using Microsoft.Extensions.DependencyInjection;
using ReservationService.Core.Interfaces;

namespace ReservationService.Database.Repositories.Extensions;

public static class ProviderExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IReservationRepository, ReservationRepository>();
    }
}