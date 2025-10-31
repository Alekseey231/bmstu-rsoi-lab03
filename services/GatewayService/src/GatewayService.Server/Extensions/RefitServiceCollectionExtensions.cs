using GatewayService.Clients;
using GatewayService.Server.Configurations;
using GatewayService.Server.Handlers;
using GatewayService.Services.CircuitBreaker;
using Microsoft.Extensions.Options;
using Refit;

namespace GatewayService.Server.Extensions;

public static class RefitServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClients(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var httpConfig = configuration.GetSection(nameof(HttpClientConfig))
            .Get<HttpClientConfig>();
        var cbConfig = configuration.GetSection(nameof(CircuitBreakerConfig))
            .Get<CircuitBreakerConfig>();

        if (httpConfig == null)
        {
            throw new InvalidOperationException(
                $"Configuration section '{nameof(HttpClientConfig)}' not found");
        }
        
        services.AddSingleton<CircuitBreakersCache>();
        
        services.AddRefitClient<ILibraryServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(httpConfig.LibraryServiceUrl))
            .AddHttpMessageHandler(sp => CreateHandler(sp, "LibraryService", cbConfig));

        services.AddRefitClient<IRatingServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(httpConfig.RatingServiceUrl))
            .AddHttpMessageHandler(sp => CreateHandler(sp, "RatingService", cbConfig));

        services.AddRefitClient<IReservationServiceClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(httpConfig.ReservationServiceUrl))
            .AddHttpMessageHandler(sp => CreateHandler(sp, "ReservationService", cbConfig));

        return services;
    }

    private static CircuitBreakerHttpMessageHandler CreateHandler(
        IServiceProvider sp,
        string serviceName,
        CircuitBreakerConfig? config)
    {
        var cache = sp.GetRequiredService<CircuitBreakersCache>();
        var settings = config?.Services.GetValueOrDefault(serviceName) 
            ?? new ServiceCircuitBreakerSettings();
        
        try
        {
            cache.Register(serviceName, builder => builder
                .WithFailureThreshold(settings.FailureThreshold)
                .WithBreakDuration(TimeSpan.FromSeconds(settings.BreakDurationSeconds))
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .Or<TimeoutException>());
        }
        catch (InvalidOperationException)
        {

        }

        return new CircuitBreakerHttpMessageHandler(cache, serviceName);
    }
}