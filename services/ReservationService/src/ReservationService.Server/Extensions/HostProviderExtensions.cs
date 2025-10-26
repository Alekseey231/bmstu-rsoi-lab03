using Microsoft.EntityFrameworkCore;
using ReservationService.Database.Context;

namespace ReservationService.Server.Extensions;

public static class HostProviderExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<ReservationServiceContext>()!;
        
        context.Database.Migrate();
        
        return host;
    }

}