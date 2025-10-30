using GatewayService.Services.CircuitBreaker.Exceptions;
using GatewayService.Services.CircuitBreaker.Models;
using Microsoft.Extensions.Logging;

namespace GatewayService.Services.CircuitBreaker;

public class CircuitBreaker : IDisposable
{
    private readonly string _serviceName;
    private readonly int _failureThreshold;
    private readonly TimeSpan _breakDuration;
    private readonly ILogger<CircuitBreaker> _logger;
    private readonly Func<Exception, bool> _shouldHandleException;
    private readonly SemaphoreSlim _semaphore;
    
    private int _failureCount;
    private CircuitBreakerState _state;
    private DateTimeOffset? _openedAt;
    
    internal CircuitBreaker(
        string serviceName,
        int failureThreshold,
        TimeSpan breakDuration,
        ILogger<CircuitBreaker> logger,
        Func<Exception, bool> shouldHandleException)
    {
        _serviceName = serviceName;
        _failureThreshold = failureThreshold;
        _breakDuration = breakDuration;
        _logger = logger;
        _shouldHandleException = shouldHandleException;
        _semaphore = new SemaphoreSlim(1, 1);
        
        _failureCount = 0;
        _state = CircuitBreakerState.Closed;
        _openedAt = null;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        await CheckStateAsync();

        try
        {
            var result = await action();
            await RegisterSuccessAsync();
            return result;
        }
        catch (Exception ex)
        {
            if (_shouldHandleException(ex))
            {
                await RegisterFailureAsync();
            }
            
            throw;
        }
    }
    
    private async Task CheckStateAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            switch (_state)
            {
                case CircuitBreakerState.Closed:
                    break;
                    
                case CircuitBreakerState.Open:
                    if (IsBreakDurationExpired())
                    {
                        TransitionToHalfOpen();
                    }
                    else
                    {
                        throw new BrokenCircuitException(_serviceName);
                    }
                    break;
                    
                case CircuitBreakerState.HalfOpen:
                    break;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private async Task RegisterSuccessAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            _logger.LogDebug("[{ServiceName}] Request succeeded in state: {State}", _serviceName, _state);
            
            if (_state == CircuitBreakerState.HalfOpen)
            {
                TransitionToClosed();
            }
            else if (_state == CircuitBreakerState.Closed)
            {
                _failureCount = 0;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private async Task RegisterFailureAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            _failureCount++;
            
            _logger.LogWarning("[{ServiceName}] Request failed in state: {State}, failure count: {Count}/{Threshold}",
                _serviceName, 
                _state, 
                _failureCount, 
                _failureThreshold);
            
            if (_state == CircuitBreakerState.HalfOpen || _failureCount >= _failureThreshold)
            {
                TransitionToOpen();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private void TransitionToOpen()
    {
        _logger.LogWarning("[{ServiceName}] Circuit breaker: {OldState} -> OPEN (blocking for {Duration}s)",
            _serviceName, 
            _state, 
            _breakDuration.TotalSeconds);
        
        _state = CircuitBreakerState.Open;
        _openedAt = DateTimeOffset.UtcNow;
    }

    private void TransitionToHalfOpen()
    {
        _logger.LogInformation("[{ServiceName}] Circuit breaker: OPEN -> HALF_OPEN (testing recovery)",
            _serviceName);
        
        _state = CircuitBreakerState.HalfOpen;
    }

    private void TransitionToClosed()
    {
        _logger.LogInformation("[{ServiceName}] Circuit breaker: {OldState} -> CLOSED (normal operation)",
            _serviceName, _state);
        
        _state = CircuitBreakerState.Closed;
        _failureCount = 0;
        _openedAt = null;
    }
    
    // ========== Вспомогательные методы ==========
    
    private bool IsBreakDurationExpired()
    {
        return _openedAt.HasValue && DateTimeOffset.UtcNow >= _openedAt.Value.Add(_breakDuration);
    }

    public async Task<CircuitBreakerState> GetStateAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return _state;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}