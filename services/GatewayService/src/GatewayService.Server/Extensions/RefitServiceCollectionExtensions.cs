using GatewayService.Server.Clients;
using GatewayService.Server.Configurations;
using Microsoft.Extensions.Options;
using Refit;

namespace GatewayService.Server.Extensions;

public static class RefitServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClients(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(HttpClientConfig)).Get<HttpClientConfig>();
        
        services.AddRefitClient<ILibraryServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(config!.LibraryServiceUrl));

        services.AddRefitClient<IRatingServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(config!.RatingServiceUrl));

        services.AddRefitClient<IReservationServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(config!.ReservationServiceUrl));

        return services;
    }
}