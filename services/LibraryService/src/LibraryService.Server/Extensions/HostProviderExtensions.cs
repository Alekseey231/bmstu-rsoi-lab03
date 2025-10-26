using Microsoft.EntityFrameworkCore;
using LibraryService.Database.Context;

namespace LibraryService.Server.Extensions;

public static class HostProviderExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<LibraryServiceContext>()!;
        
        context.Database.Migrate();
        
        return host;
    }

}