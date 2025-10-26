using LibraryService.Core.Interfaces;
using LibraryService.Core.Models;
using LibraryService.Core.Models.Enums;

namespace LibraryService.Tests.Services;

using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LibraryServiceTests
{
    private readonly Mock<ILibraryRepository> _mockRepository;
    private readonly Mock<ILogger<LibraryService.Services.LibraryService.LibraryService>> _mockLogger;
    private readonly LibraryService.Services.LibraryService.LibraryService _service;

    public LibraryServiceTests()
    {
        _mockRepository = new Mock<ILibraryRepository>();
        _mockLogger = new Mock<ILogger<LibraryService.Services.LibraryService.LibraryService>>();
        _service = new LibraryService.Services.LibraryService.LibraryService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllLibrariesAsync_ShouldReturnLibraries_WhenCalled()
    {
        // Arrange
        var city = "Moscow";
        var expectedLibraries = new List<Library>
        {
            new(Guid.NewGuid(), "Central Library", city, "Red Square 1"),
            new(Guid.NewGuid(), "District Library", city, "Lenin Street 25")
        };

        _mockRepository
            .Setup(r => r.GetAllLibrariesAsync(city, null, null))
            .ReturnsAsync(expectedLibraries);

        // Act
        var result = await _service.GetAllLibrariesAsync(city);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedLibraries, result);
        _mockRepository.Verify(r => r.GetAllLibrariesAsync(city, null, null), Times.Once);
    }

    [Fact]
    public async Task GetCountOfLibrariesAsync_ShouldReturnCount_WhenCalled()
    {
        // Arrange
        var city = "Moscow";
        var expectedCount = 5;

        _mockRepository
            .Setup(r => r.GetCountOfLibrariesAsync(city))
            .ReturnsAsync(expectedCount);

        // Act
        var result = await _service.GetCountOfLibrariesAsync(city);

        // Assert
        Assert.Equal(expectedCount, result);
        _mockRepository.Verify(r => r.GetCountOfLibrariesAsync(city), Times.Once);
    }

    [Fact]
    public async Task CheckOutBookAsync_ShouldReturnInventoryItem_WhenBookIsAvailable()
    {
        // Arrange
        var libraryId = Guid.NewGuid();
        var bookUid = Guid.NewGuid();
        var book = new Book(bookUid, "Clean Code", "Robert Martin", "Programming", BookCondition.Excellent);
        var expectedItem = new InventoryItem(book, 4);

        _mockRepository
            .Setup(r => r.CheckOutBookAsync(libraryId, bookUid))
            .ReturnsAsync(expectedItem);

        // Act
        var result = await _service.CheckOutBookAsync(libraryId, bookUid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book, result.Book);
        Assert.Equal(4, result.AvailableCount);
        _mockRepository.Verify(r => r.CheckOutBookAsync(libraryId, bookUid), Times.Once);
    }

    [Fact]
    public async Task CheckInBookAsync_ShouldReturnCheckInResult_WhenBookIsReturned()
    {
        // Arrange
        var libraryId = Guid.NewGuid();
        var bookUid = Guid.NewGuid();
        var bookCondition = BookCondition.Good;
        var book = new Book(bookUid, "Design Patterns", "Gang of Four", "Programming", BookCondition.Good);
        var oldInventory = new InventoryItem(book, 3);
        var newInventory = new InventoryItem(book, 4);

        _mockRepository
            .Setup(r => r.GetBookByIdAsync(libraryId, bookUid))
            .ReturnsAsync(oldInventory);

        _mockRepository
            .Setup(r => r.CheckInBookAsync(libraryId, bookUid, bookCondition))
            .ReturnsAsync(newInventory);

        // Act
        var result = await _service.CheckInBookAsync(libraryId, bookUid, bookCondition);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(oldInventory, result.OldItem);
        Assert.Equal(newInventory, result.NewItem);
        _mockRepository.Verify(r => r.GetBookByIdAsync(libraryId, bookUid), Times.Once);
        _mockRepository.Verify(r => r.CheckInBookAsync(libraryId, bookUid, bookCondition), Times.Once);
    }

    [Fact]
    public async Task GetLibraryBooksAsync_ShouldReturnBooks_WhenLibraryExists()
    {
        // Arrange
        var libraryUid = Guid.NewGuid();
        var showAll = true;
        var expectedBooks = new List<InventoryItem>
        {
            new(new Book(Guid.NewGuid(), "Book 1", "Author 1", "Fiction", BookCondition.Excellent), 5),
            new(new Book(Guid.NewGuid(), "Book 2", "Author 2", "Science", BookCondition.Good), 3)
        };

        _mockRepository
            .Setup(r => r.GetLibraryBooksAsync(libraryUid, showAll, null, null))
            .ReturnsAsync(expectedBooks);

        // Act
        var result = await _service.GetLibraryBooksAsync(libraryUid, showAll);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedBooks, result);
        _mockRepository.Verify(r => r.GetLibraryBooksAsync(libraryUid, showAll, null, null), Times.Once);
    }

    [Fact]
    public async Task GetLibraryByIdAsync_ShouldReturnLibrary_WhenLibraryExists()
    {
        // Arrange
        var libraryId = Guid.NewGuid();
        var expectedLibrary = new Library(libraryId, "Main Library", "Saint Petersburg", "Nevsky Prospect 1");

        _mockRepository
            .Setup(r => r.GetLibraryByIdAsync(libraryId))
            .ReturnsAsync(expectedLibrary);

        // Act
        var result = await _service.GetLibraryByIdAsync(libraryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(libraryId, result.LibraryId);
        Assert.Equal("Main Library", result.Name);
        Assert.Equal("Saint Petersburg", result.City);
        _mockRepository.Verify(r => r.GetLibraryByIdAsync(libraryId), Times.Once);
    }
}