using Microsoft.Extensions.DependencyInjection;
using ReservationService.Core.Interfaces;

namespace ReservationService.Services.ReservationService.Extensions;

public static class ProviderExtensions
{
    public static void AddPersonService(this IServiceCollection services)
    {
        services.AddScoped<IReservationService, ReservationService>();
    }
}
