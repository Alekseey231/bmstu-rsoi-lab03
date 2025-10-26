using Microsoft.Extensions.DependencyInjection;
using LibraryService.Core.Interfaces;

namespace LibraryService.Services.LibraryService.Extensions;

public static class ProviderExtensions
{
    public static void AddLibraryService(this IServiceCollection services)
    {
        services.AddScoped<ILibraryService, LibraryService>();
    }
}
