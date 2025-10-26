
using ReservationService.Core.Interfaces;
using ReservationService.Core.Models;
using ReservationService.Core.Models.Enums;

namespace ReservationService.Tests.Services;

using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _mockRepository;
    private readonly Mock<ILogger<ReservationService.Services.ReservationService.ReservationService>> _mockLogger;
    private readonly ReservationService.Services.ReservationService.ReservationService _service;

    public ReservationServiceTests()
    {
        _mockRepository = new Mock<IReservationRepository>();
        _mockLogger = new Mock<ILogger<ReservationService.Services.ReservationService.ReservationService>>();
        _service = new ReservationService.Services.ReservationService.ReservationService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldReturnReservation_WhenCalled()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userName = "john_doe";
        var bookId = Guid.NewGuid();
        var libraryId = Guid.NewGuid();
        var status = ReservationStatus.Rented;
        var startDate = DateTime.UtcNow;
        var tillDate = startDate.AddDays(14);

        var expectedReservation = new Reservation(
            reservationId, userName, bookId, libraryId, status, startDate, tillDate);

        _mockRepository
            .Setup(r => r.CreateReservationAsync(reservationId, userName, bookId, libraryId, status, startDate, tillDate))
            .ReturnsAsync(expectedReservation);

        // Act
        var result = await _service.CreateReservationAsync(
            reservationId, userName, bookId, libraryId, status, startDate, tillDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservationId, result.ReservationId);
        Assert.Equal(userName, result.UserName);
        Assert.Equal(bookId, result.BookId);
        Assert.Equal(libraryId, result.LibraryId);
        Assert.Equal(status, result.Status);
        _mockRepository.Verify(
            r => r.CreateReservationAsync(reservationId, userName, bookId, libraryId, status, startDate, tillDate), 
            Times.Once);
    }

    [Fact]
    public async Task UpdateReservationAsync_ShouldSetStatusToReturned_WhenReturnedOnTime()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-10);
        var tillDate = DateTime.UtcNow.AddDays(4);
        var returnedDate = DateTime.UtcNow;

        var existingReservation = new Reservation(
            reservationId, "user1", Guid.NewGuid(), Guid.NewGuid(), 
            ReservationStatus.Rented, startDate, tillDate);

        var updatedReservation = new Reservation(
            reservationId, "user1", existingReservation.BookId, existingReservation.LibraryId,
            ReservationStatus.Returned, startDate, tillDate);

        _mockRepository
            .Setup(r => r.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(existingReservation);

        _mockRepository
            .Setup(r => r.UpdateReservationAsync(reservationId, ReservationStatus.Returned))
            .ReturnsAsync(updatedReservation);

        // Act
        var result = await _service.UpdateReservationAsync(reservationId, returnedDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ReservationStatus.Returned, result.Status);
        _mockRepository.Verify(r => r.GetReservationByIdAsync(reservationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateReservationAsync(reservationId, ReservationStatus.Returned), Times.Once);
    }

    [Fact]
    public async Task UpdateReservationAsync_ShouldSetStatusToExpired_WhenReturnedLate()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-20);
        var tillDate = DateTime.UtcNow.AddDays(-5);
        var returnedDate = DateTime.UtcNow;

        var existingReservation = new Reservation(
            reservationId, "user2", Guid.NewGuid(), Guid.NewGuid(),
            ReservationStatus.Rented, startDate, tillDate);

        var updatedReservation = new Reservation(
            reservationId, "user2", existingReservation.BookId, existingReservation.LibraryId,
            ReservationStatus.Expired, startDate, tillDate);

        _mockRepository
            .Setup(r => r.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(existingReservation);

        _mockRepository
            .Setup(r => r.UpdateReservationAsync(reservationId, ReservationStatus.Expired))
            .ReturnsAsync(updatedReservation);

        // Act
        var result = await _service.UpdateReservationAsync(reservationId, returnedDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ReservationStatus.Expired, result.Status);
        _mockRepository.Verify(r => r.GetReservationByIdAsync(reservationId), Times.Once);
        _mockRepository.Verify(r => r.UpdateReservationAsync(reservationId, ReservationStatus.Expired), Times.Once);
    }

    [Fact]
    public async Task GetReservationByUserNameAsync_ShouldReturnReservations_WhenUserExists()
    {
        // Arrange
        var userName = "jane_smith";
        var expectedReservations = new List<Reservation>
        {
            new Reservation(Guid.NewGuid(), userName, Guid.NewGuid(), Guid.NewGuid(), 
                ReservationStatus.Rented, DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(9)),
            new Reservation(Guid.NewGuid(), userName, Guid.NewGuid(), Guid.NewGuid(),
                ReservationStatus.Returned, DateTime.UtcNow.AddDays(-20), DateTime.UtcNow.AddDays(-6))
        };

        _mockRepository
            .Setup(r => r.GetReservationByUserNameAsync(userName, null))
            .ReturnsAsync(expectedReservations);

        // Act
        var result = await _service.GetReservationByUserNameAsync(userName, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.Equal(userName, r.UserName));
        _mockRepository.Verify(r => r.GetReservationByUserNameAsync(userName, null), Times.Once);
    }

    [Fact]
    public async Task GetReservationByUserNameAsync_ShouldReturnFilteredReservations_WhenStatusProvided()
    {
        // Arrange
        var userName = "bob_jones";
        var status = ReservationStatus.Rented;
        var expectedReservations = new List<Reservation>
        {
            new Reservation(Guid.NewGuid(), userName, Guid.NewGuid(), Guid.NewGuid(),
                ReservationStatus.Rented, DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddDays(11))
        };

        _mockRepository
            .Setup(r => r.GetReservationByUserNameAsync(userName, status))
            .ReturnsAsync(expectedReservations);

        // Act
        var result = await _service.GetReservationByUserNameAsync(userName, status);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(ReservationStatus.Rented, result[0].Status);
        _mockRepository.Verify(r => r.GetReservationByUserNameAsync(userName, status), Times.Once);
    }

    [Fact]
    public async Task GetReservationByIdAsync_ShouldReturnReservation_WhenReservationExists()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var expectedReservation = new Reservation(
            reservationId, "test_user", Guid.NewGuid(), Guid.NewGuid(),
            ReservationStatus.Rented, DateTime.UtcNow, DateTime.UtcNow.AddDays(14));

        _mockRepository
            .Setup(r => r.GetReservationByIdAsync(reservationId))
            .ReturnsAsync(expectedReservation);

        // Act
        var result = await _service.GetReservationByIdAsync(reservationId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservationId, result.ReservationId);
        Assert.Equal("test_user", result.UserName);
        _mockRepository.Verify(r => r.GetReservationByIdAsync(reservationId), Times.Once);
    }
}