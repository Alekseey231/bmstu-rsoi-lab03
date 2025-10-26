using Microsoft.EntityFrameworkCore;
using RatingService.Database.Context;

namespace RatingService.Server.Extensions;

public static class HostProviderExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<RatingServiceContext>()!;
        
        context.Database.Migrate();
        
        return host;
    }

}