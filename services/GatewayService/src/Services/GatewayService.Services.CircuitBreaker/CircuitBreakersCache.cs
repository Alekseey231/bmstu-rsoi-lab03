using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace GatewayService.Services.CircuitBreaker;

/// <summary>
/// Кэш Circuit Breaker'ов для всех сервисов
/// </summary>
public class CircuitBreakersCache
{
    private readonly ConcurrentDictionary<string, CircuitBreaker> _servicesCircuitBreakers;
    private readonly ILogger<CircuitBreaker> _logger;

    public CircuitBreakersCache(ILogger<CircuitBreaker> logger)
    {
        _logger = logger;
        _servicesCircuitBreakers = new ConcurrentDictionary<string, CircuitBreaker>();
    }
    
    public void Register(string serviceName,
        Action<CircuitBreakerBuilder> configure)
    {
        if (_servicesCircuitBreakers.ContainsKey(serviceName))
            throw new InvalidOperationException($"Circuit breaker for service '{serviceName}' already registered");

        var builder = new CircuitBreakerBuilder(serviceName);
        configure(builder);
        
        var circuitBreaker = builder.Build(_logger);
        
        if (!_servicesCircuitBreakers.TryAdd(serviceName, circuitBreaker))
        {
            throw new InvalidOperationException($"Failed to register circuit breaker for service '{serviceName}'");
        }
    }
    
    public CircuitBreaker GetCircuitBreakerForService(string serviceName)
    {
        if (_servicesCircuitBreakers.TryGetValue(serviceName, out var circuitBreaker))
        {
            return circuitBreaker;
        }

        throw new InvalidOperationException($"Circuit breaker for service '{serviceName}' not found. " +
                                            $"Make sure to register it first using Register() method.");
    }
    
    public CircuitBreaker? FindCircuitBreakerForService(string serviceName)
    {
        return _servicesCircuitBreakers.GetValueOrDefault(serviceName);
    }
    
    public IReadOnlyDictionary<string, CircuitBreaker> GetAll()
    {
        return _servicesCircuitBreakers;
    }
}