namespace GatewayService.Services.CircuitBreaker.Exceptions;

public class BrokenCircuitException : Exception
{
    public string ServiceName { get; }
    
    public BrokenCircuitException(string serviceName) 
        : base($"Circuit breaker for {serviceName} is OPEN")
    {
        ServiceName = serviceName;
    }
}