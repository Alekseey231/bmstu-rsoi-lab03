using GatewayService.Services.CircuitBreaker;

namespace GatewayService.Server.Handlers;

/// <summary>
/// HTTP handler для интеграции Circuit Breaker с Refit клиентами
/// </summary>
public class CircuitBreakerHttpMessageHandler : DelegatingHandler
{
    private readonly CircuitBreakersCache _cache;
    private readonly string _serviceName;

    public CircuitBreakerHttpMessageHandler(CircuitBreakersCache cache,
        string serviceName)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _serviceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        var circuitBreaker = _cache.GetCircuitBreakerForService(_serviceName);
        
        return await circuitBreaker.ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));
    }
}