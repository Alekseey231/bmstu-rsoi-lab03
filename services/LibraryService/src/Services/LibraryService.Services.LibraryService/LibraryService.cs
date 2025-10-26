using LibraryService.Core;
using Microsoft.Extensions.Logging;
using LibraryService.Core.Interfaces;
using LibraryService.Core.Models;
using LibraryService.Core.Models.Enums;

namespace LibraryService.Services.LibraryService;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<LibraryService> _logger;

    public LibraryService(ILibraryRepository libraryRepository, ILogger<LibraryService> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<List<Library>> GetAllLibrariesAsync(string city, int? limit = null, int? offset = null)
    {
        _logger.LogDebug("Getting all libraries");
        
        var result = await _libraryRepository.GetAllLibrariesAsync(city, limit, offset);
        
        _logger.LogInformation("Got {Count} libraries", result.Count);
        
        return result;
    }

    public async Task<int> GetCountOfLibrariesAsync(string city)
    {
        _logger.LogDebug("Getting count of libraries");
        
        var result = await _libraryRepository.GetCountOfLibrariesAsync(city);
        
        _logger.LogInformation("Got {Count} libraries", result);
        
        return result;
    }

    public async Task<List<InventoryItem>> GetLibraryBooksAsync(Guid libraryUid, bool? showAll, int? offset = null, int? limit = null)
    {
        _logger.LogDebug("Getting library books");
        
        var result = await _libraryRepository.GetLibraryBooksAsync(libraryUid, showAll, offset, limit);
        
        _logger.LogInformation("Got {Count} library books", result.Count);
        
        return result;
    }

    public async Task<int> GetCountOfLibraryBooksAsync(Guid libraryUid, bool? showAll)
    {
        _logger.LogDebug("Getting count of library books");
        
        var result = await _libraryRepository.GetCountOfLibraryBooksAsync(libraryUid, showAll);
        
        _logger.LogInformation("Got {Count} library books", result);
        
        return result;
    }

    public async Task<InventoryItem> CheckOutBookAsync(Guid libraryId, Guid bookUid)
    {
        _logger.LogDebug("Checking out book {BookUid} in library {LibraryId}", bookUid, libraryId);
        
        var result = await _libraryRepository.CheckOutBookAsync(libraryId, bookUid);
        
        _logger.LogInformation("Book {BookUid} has been checked out int library {LibraryId}", bookUid, libraryId);

        return result;
    }

    public async Task<CheckInResult> CheckInBookAsync(Guid libraryId, Guid bookUid, BookCondition bookCondition)
    {
        _logger.LogDebug("Checking in book {BookUid} in library {LibraryId}", bookUid, libraryId);
        
        var oldBook = await _libraryRepository.GetBookByIdAsync(libraryId, bookUid);
        
        var result = await _libraryRepository.CheckInBookAsync(libraryId, bookUid, bookCondition);
        
        _logger.LogInformation("Book {BookUid} has been checked in int library {LibraryId}", bookUid, libraryId);

        return new CheckInResult(oldBook, result);
    }

    public async Task<InventoryItem> GetBookByIdAsync(Guid libraryId, Guid bookUid)
    {
        _logger.LogDebug("Getting book {BookUid} from library {LibraryId}", bookUid, libraryId);
        
        var result = await _libraryRepository.GetBookByIdAsync(libraryId, bookUid);
        
        _logger.LogInformation("Got book {BookUid} from library {LibraryId}", bookUid, libraryId);
        
        return result;
    }
    
    public async Task<Library> GetLibraryByIdAsync(Guid libraryId)
    {
        _logger.LogDebug("Getting library {LibraryId}", libraryId);
        
        var result = await _libraryRepository.GetLibraryByIdAsync(libraryId);
        
        _logger.LogInformation("Got library {LibraryId}", libraryId);
        
        return result;
    }

    public async Task<List<BookWithLibrary>> GetBooksWithLibrariesAsync(List<Guid> bookIds)
    {
        _logger.LogDebug("Getting books with libraries");
        
        var result = await _libraryRepository.GetBooksWithLibrariesAsync(bookIds);
        
        _logger.LogInformation("Got books with libraries");
        
        return result;
    }
}