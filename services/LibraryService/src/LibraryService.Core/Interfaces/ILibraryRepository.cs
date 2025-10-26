using LibraryService.Core.Models;
using LibraryService.Core.Models.Enums;

namespace LibraryService.Core.Interfaces;

public interface ILibraryRepository
{
    Task<List<Library>> GetAllLibrariesAsync(string city, int? limit = null, int? offset = null);
    
    Task<int> GetCountOfLibrariesAsync(string city);
    
    Task<List<InventoryItem>> GetLibraryBooksAsync(Guid libraryUid, bool? showAll, int? offset = null, int? limit = null);
    
    Task<int> GetCountOfLibraryBooksAsync(Guid libraryUid, bool? showAll);
    
    Task<InventoryItem> CheckOutBookAsync(Guid libraryId, Guid bookUid);
    
    Task<InventoryItem> CheckInBookAsync(Guid libraryId, Guid bookUid, BookCondition bookCondition);

    Task<InventoryItem> GetBookByIdAsync(Guid libraryId, Guid bookUid);
    
    Task<Library> GetLibraryByIdAsync(Guid libraryId);
    
    Task<List<BookWithLibrary>> GetBooksWithLibrariesAsync(List<Guid> bookIds);
}