using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace GatewayService.Services.CircuitBreaker;

public class CircuitBreakerBuilder
{
    private readonly string _serviceName;
    private int _failureThreshold = 5;
    private TimeSpan _breakDuration = TimeSpan.FromSeconds(30);
    private readonly List<Func<Exception, bool>> _exceptionPredicates = new();
    
    public CircuitBreakerBuilder(string serviceName)
    {
        _serviceName = serviceName;
    }
    
    public CircuitBreakerBuilder WithFailureThreshold(int threshold)
    {
        if (threshold <= 0)
            throw new ArgumentException("Threshold must be positive", nameof(threshold));
        
        _failureThreshold = threshold;
        return this;
    }
    
    public CircuitBreakerBuilder WithBreakDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));
        
        _breakDuration = duration;
        return this;
    }
    
    public CircuitBreakerBuilder Handle<TException>() 
        where TException : Exception
    {
        _exceptionPredicates.Add(ex => ex is TException);
        return this;
    }
    
    public CircuitBreakerBuilder Handle<TException>(Func<TException, bool> predicate) 
        where TException : Exception
    {
        _exceptionPredicates.Add(ex => ex is TException typedEx && predicate(typedEx));
        return this;
    }
    
    public CircuitBreakerBuilder Or<TException>() 
        where TException : Exception
    {
        return Handle<TException>();
    }
    
    public CircuitBreakerBuilder Or<TException>(Func<TException, bool> predicate) 
        where TException : Exception
    {
        return Handle(predicate);
    }
    
    public CircuitBreaker Build(ILogger<CircuitBreaker> logger)
    {
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));
        
        Func<Exception, bool> shouldHandleException;
        
        if (_exceptionPredicates.Count != 0)
        {
            shouldHandleException = ex => _exceptionPredicates.Any(predicate => predicate(ex));
        }
        else
        {
            shouldHandleException = DefaultExceptionPredicate;
        }
        
        return new CircuitBreaker(
            _serviceName,
            _failureThreshold,
            _breakDuration,
            logger,
            shouldHandleException);
    }
    
    private static bool DefaultExceptionPredicate(Exception ex)
    {
        return ex is HttpRequestException 
            or TaskCanceledException 
            or OperationCanceledException 
            or TimeoutException 
            or SocketException;
    }
}