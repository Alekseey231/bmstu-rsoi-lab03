namespace GatewayService.Services.CircuitBreaker.Models;

public enum CircuitBreakerState
{
    /// <summary>
    /// Разомкнутое состояние.
    /// </summary>
    Open = 0,
    
    /// <summary>
    /// Наполовину разомкнутое состояние.
    /// </summary>
    HalfOpen = 1,
    
    /// <summary>
    /// Замкнутое состояние.
    /// </summary>
    Closed = 2,
}