using GatewayService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GatewayService.Services.RequestsQueue.Extensions;

public static class ProviderExtensions
{
    public static void AddRequestsQueues(this IServiceCollection services)
    {
        services.AddSingleton<ILibraryServiceRequestsQueue, LibraryServiceRequestsQueue>();
        services.AddSingleton<IRatingServiceRequestsQueue, RatingServiceRequestsQueue>();
    }
}