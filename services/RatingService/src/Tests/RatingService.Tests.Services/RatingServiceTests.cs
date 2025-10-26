using RatingService.Core.Interfaces;
using RatingService.Core.Models;

namespace RatingService.Tests.Services;

using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class RatingServiceTests
{
    private readonly Mock<IRatingRepository> _mockRepository;
    private readonly Mock<ILogger<RatingService.Services.RatingService.RatingService>> _mockLogger;
    private readonly RatingService.Services.RatingService.RatingService _service;

    public RatingServiceTests()
    {
        _mockRepository = new Mock<IRatingRepository>();
        _mockLogger = new Mock<ILogger<RatingService.Services.RatingService.RatingService>>();
        _service = new RatingService.Services.RatingService.RatingService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetRatingByUserNameAsync_ShouldReturnRating_WhenUserExists()
    {
        // Arrange
        var userName = "john_doe";
        var expectedRating = new Rating(userName, 75);

        _mockRepository
            .Setup(r => r.GetRatingByUserNameAsync(userName))
            .ReturnsAsync(expectedRating);

        // Act
        var result = await _service.GetRatingByUserNameAsync(userName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.UserName);
        Assert.Equal(75, result.Stars);
        _mockRepository.Verify(r => r.GetRatingByUserNameAsync(userName), Times.Once);
    }

    [Fact]
    public async Task UpdateRatingAsync_ShouldReturnUpdatedRating_WhenStarsAreValid()
    {
        // Arrange
        var userName = "jane_smith";
        var stars = 85;
        var expectedRating = new Rating(userName, stars);

        _mockRepository
            .Setup(r => r.UpdateRatingAsync(userName, stars))
            .ReturnsAsync(expectedRating);

        // Act
        var result = await _service.UpdateRatingAsync(userName, stars);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.UserName);
        Assert.Equal(stars, result.Stars);
        _mockRepository.Verify(r => r.UpdateRatingAsync(userName, stars), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task UpdateRatingAsync_ShouldAcceptValidStarValues(int stars)
    {
        // Arrange
        var userName = "test_user";
        var expectedRating = new Rating(userName, stars);

        _mockRepository
            .Setup(r => r.UpdateRatingAsync(userName, stars))
            .ReturnsAsync(expectedRating);

        // Act
        var result = await _service.UpdateRatingAsync(userName, stars);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stars, result.Stars);
        _mockRepository.Verify(r => r.UpdateRatingAsync(userName, stars), Times.Once);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(101)]
    [InlineData(200)]
    public async Task UpdateRatingAsync_ShouldThrowArgumentOutOfRangeException_WhenStarsAreInvalid(int stars)
    {
        // Arrange
        var userName = "test_user";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _service.UpdateRatingAsync(userName, stars)
        );

        Assert.Equal("stars", exception.ParamName);
        Assert.Contains("Stars must be between 0 and 100", exception.Message);
        _mockRepository.Verify(r => r.UpdateRatingAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRatingAsync_ShouldNotCallRepository_WhenStarsAreBelowZero()
    {
        // Arrange
        var userName = "test_user";
        var invalidStars = -5;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _service.UpdateRatingAsync(userName, invalidStars)
        );

        _mockRepository.Verify(r => r.UpdateRatingAsync(userName, invalidStars), Times.Never);
    }

    [Fact]
    public async Task UpdateRatingAsync_ShouldNotCallRepository_WhenStarsAreAbove100()
    {
        // Arrange
        var userName = "test_user";
        var invalidStars = 150;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => _service.UpdateRatingAsync(userName, invalidStars)
        );

        _mockRepository.Verify(r => r.UpdateRatingAsync(userName, invalidStars), Times.Never);
    }
}