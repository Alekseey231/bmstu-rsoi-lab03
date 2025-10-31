using LibraryService.Dto.Http;
using LibraryService.Dto.Http.Models;
using LibraryService.Dto.Http.Models.Enums;
using Refit;

namespace GatewayService.Clients;

public interface ILibraryServiceClient
{
    [Get("/api/v1/libraries")]
    Task<LibraryPaginationResponse> GetLibrariesAsync(
        [Query] string city,
        [Query] int? page = null,
        [Query] int? size = null);

    [Get("/api/v1/libraries/{libraryUid}/books")]
    Task<LibraryBookPaginationResponse> GetBooksAsync(
        Guid libraryUid,
        [Query] bool? showAll = null,
        [Query] int? page = null,
        [Query] int? size = null);
    
    [Get("/api/v1/libraries/{libraryUid}/books/{bookUid}")]
    Task<BookWithLibrary> GetBookAsync(Guid libraryUid, Guid bookUid);

    [Post("/api/v1/libraries/{libraryUid}/books/{bookUid}/checkout")]
    Task<LibraryBook> CheckOutBookAsync(Guid libraryUid, Guid bookUid);

    [Post("/api/v1/libraries/{libraryUid}/books/{bookUid}/checkin")]
    Task<CheckInBookResponse> CheckInBookAsync(Guid libraryUid, Guid bookUid,
        [Body] BookCondition bookCondition);
    
    [Get("/api/v1/libraries/books")]
    Task<List<BookWithLibrary>> GetBooksByIds([Query] string sourceIds);
}