using Microsoft.Extensions.DependencyInjection;

namespace GatewayService.Services.RequestsProcessingBackgroundService.Extensions;

public static class ProviderExtensions
{
    public static void AddRequestsBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<LibraryServiceQueueBackgroundService>();
        services.AddHostedService<RatingServiceQueueBackgroundService>();
    }
}