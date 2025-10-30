namespace GatewayService.Server.Configurations;

public class CircuitBreakerConfig
{
    public Dictionary<string, ServiceCircuitBreakerSettings> Services { get; set; } = new();
}