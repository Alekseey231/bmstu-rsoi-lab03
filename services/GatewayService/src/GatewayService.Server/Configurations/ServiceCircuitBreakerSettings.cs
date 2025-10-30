namespace GatewayService.Server.Configurations;

public class ServiceCircuitBreakerSettings
{
    public int FailureThreshold { get; set; }
    public int BreakDurationSeconds { get; set; }

    public ServiceCircuitBreakerSettings()
    {
        FailureThreshold = 5;
        BreakDurationSeconds = 30;
    }
}