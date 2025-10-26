using Microsoft.Extensions.DependencyInjection;
using LibraryService.Core.Interfaces;

namespace LibraryService.Database.Repositories.Extensions;

public static class ProviderExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILibraryRepository, LibraryRepository>();
    }
}