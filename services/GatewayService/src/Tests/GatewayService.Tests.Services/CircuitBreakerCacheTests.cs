using GatewayService.Services.CircuitBreaker;
using GatewayService.Services.CircuitBreaker.Exceptions;
using GatewayService.Services.CircuitBreaker.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace GatewayService.Tests.Services;

public class CircuitBreakerCacheTests
{
    private readonly CircuitBreakersCache _cache;

    public CircuitBreakerCacheTests()
    {
        Mock<ILogger<CircuitBreaker>> loggerMock = new();
        _cache = new CircuitBreakersCache(loggerMock.Object);
    }

    [Fact]
    public void Register_ShouldCreateCircuitBreakerWithCorrectSettings()
    {
        // Arrange & Act
        _cache.Register("TestService", builder => builder
            .WithFailureThreshold(3)
            .WithBreakDuration(TimeSpan.FromSeconds(10)));

        var breaker = _cache.GetCircuitBreakerForService("TestService");

        // Assert
        Assert.NotNull(breaker);
    }

    [Fact]
    public void GetCircuitBreakerForService_WhenNotRegistered_ShouldThrowException()
    {
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _cache.GetCircuitBreakerForService("NonExistent"));

        Assert.Contains("not found", exception.Message);
    }

    [Fact]
    public void Register_WhenCalledTwiceForSameService_ShouldThrowException()
    {
        // Arrange
        _cache.Register("TestService", builder => builder
            .WithFailureThreshold(5)
            .WithBreakDuration(TimeSpan.FromSeconds(30)));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _cache.Register("TestService", builder => builder
                .WithFailureThreshold(3)
                .WithBreakDuration(TimeSpan.FromSeconds(10))));

        Assert.Contains("already registered", exception.Message);
    }

    [Fact]
    public async Task CircuitBreaker_ShouldOpenAfterFailureThreshold()
    {
        // Arrange
        _cache.Register("TestService", builder => builder
            .WithFailureThreshold(3)
            .WithBreakDuration(TimeSpan.FromSeconds(5))
            .Handle<HttpRequestException>());

        var breaker = _cache.GetCircuitBreakerForService("TestService");

        // Act
        for (int i = 0; i < 3; i++)
        {
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await breaker.ExecuteAsync<string>(async () =>
                {
                    await Task.Delay(10);
                    throw new HttpRequestException("Service unavailable");
                }));
        }

        // Assert
        var state = await breaker.GetStateAsync();
        Assert.Equal(CircuitBreakerState.Open, state);

        await Assert.ThrowsAsync<BrokenCircuitException>(async () =>
            await breaker.ExecuteAsync<string>(async () =>
            {
                await Task.CompletedTask;
                return "Success";
            }));
    }

    [Fact]
    public async Task CircuitBreaker_ShouldTransitionToHalfOpen_AfterBreakDuration()
    {
        // Arrange
        _cache.Register("TestService", builder => builder
            .WithFailureThreshold(2)
            .WithBreakDuration(TimeSpan.FromSeconds(1))
            .Handle<HttpRequestException>());

        var breaker = _cache.GetCircuitBreakerForService("TestService");

        // Act
        for (int i = 0; i < 2; i++)
        {
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await breaker.ExecuteAsync<string>(async () =>
                {
                    await Task.Delay(10);
                    throw new HttpRequestException("Error");
                }));
        }

        Assert.Equal(CircuitBreakerState.Open, await breaker.GetStateAsync());
        
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        
        var result = await breaker.ExecuteAsync(async () =>
        {
            await Task.Delay(10);
            return "Success";
        });

        // Assert
        Assert.Equal("Success", result);
        Assert.Equal(CircuitBreakerState.Closed, await breaker.GetStateAsync());
    }

    [Fact]
    public void GetAll_ShouldReturnAllRegisteredCircuitBreakers()
    {
        // Arrange
        _cache.Register("Service1", builder => builder
            .WithFailureThreshold(5)
            .WithBreakDuration(TimeSpan.FromSeconds(30)));

        _cache.Register("Service2", builder => builder
            .WithFailureThreshold(3)
            .WithBreakDuration(TimeSpan.FromSeconds(60)));

        // Act
        var allBreakers = _cache.GetAll();

        // Assert
        Assert.Equal(2, allBreakers.Count);
        Assert.True(allBreakers.ContainsKey("Service1"));
        Assert.True(allBreakers.ContainsKey("Service2"));
    }
}