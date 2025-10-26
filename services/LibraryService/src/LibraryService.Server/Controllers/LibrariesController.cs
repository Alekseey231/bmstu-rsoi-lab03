using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using LibraryService.Core.Interfaces;
using LibraryService.Dto.Http;
using LibraryService.Dto.Http.Converters;
using LibraryService.Dto.Http.Converters.Enums;
using LibraryService.Dto.Http.Models;
using LibraryService.Dto.Http.Models.Enums;
using LibraryService.Server.Converters;
using LibraryService.Server.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryService.Server.Controllers;

[ApiController]
[Route("/api/v1/libraries")]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryService _libraryService;
    private readonly ILogger<LibrariesController> _logger;

    public LibrariesController(ILibraryService libraryService, 
        ILogger<LibrariesController> logger)
    {
        _libraryService = libraryService;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation("Метод для получения библиотек.", "Метод для получения библиотек.")]
    [SwaggerResponse(statusCode: 200, type: typeof(LibraryPaginationResponse), description: "Библиотеки успешно получены.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetLibraries([Required] [FromQuery] string city,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        try
        {
            page ??= 1;
            size ??= int.MaxValue;
        
            var (limit, offset) = PageSizeConverter.ToLimitOffset(page.Value, size.Value);

            //TODO: if page and size not set - don't make second request
            var libraries = await _libraryService.GetAllLibrariesAsync(city, limit, offset);
            var allLibraries = await _libraryService.GetCountOfLibrariesAsync(city);

            var dtoLibraries = libraries.ConvertAll(LibraryConverter.Convert).ToList();

            if (size == int.MaxValue)
                size = allLibraries;

            var response = new LibraryPaginationResponse(page.Value, size.Value, allLibraries, dtoLibraries);
        
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetBooks));
            
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{libraryUid:guid}/books")]
    [SwaggerOperation("Метод для получения книг в библиотеке.", "Метод для получения книг в библиотеке.")]
    [SwaggerResponse(statusCode: 200, type: typeof(LibraryBookPaginationResponse), description: "Книги успешно получены.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetBooks([Required] [FromRoute] Guid libraryUid,
        [FromQuery] bool? showAll,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        try
        {
            page ??= 1;
            size ??= int.MaxValue;
        
            var (limit, offset) = PageSizeConverter.ToLimitOffset(page.Value, size.Value);

            var inventoryItems = await _libraryService.GetLibraryBooksAsync(libraryUid, showAll, offset, limit);
            var allBooksCount = await _libraryService.GetCountOfLibraryBooksAsync(libraryUid, showAll);

            var dtoBooks = inventoryItems.ConvertAll(LibraryBookConverter.Convert).ToList();
            
            if (size == int.MaxValue)
                size = allBooksCount;

            var response = new LibraryBookPaginationResponse(page.Value, size.Value, allBooksCount, dtoBooks);
        
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetBooks));
            
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{libraryUid:guid}/books/{bookUid:guid}")]
    [SwaggerOperation("Метод для получения книги.", "Метод для получения книги.")]
    [SwaggerResponse(statusCode: 200, type: typeof(BookWithLibrary), description: "Книга успешно получена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetBook([Required] [FromRoute] Guid libraryUid,
        [Required] [FromRoute] Guid bookUid)
    {
        try
        {
            var library = await _libraryService.GetLibraryByIdAsync(libraryUid);
            var item = await _libraryService.GetBookByIdAsync(libraryUid, bookUid);
            
            var dtoBook = LibraryBookConverter.Convert(item);
            var dtoLibrary = LibraryConverter.Convert(library);

            return Ok(new BookWithLibrary(dtoBook, dtoLibrary));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetBook));
            
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("books")]
    [SwaggerOperation("Метод для получения книг.", "Метод для получения книг.")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<BookWithLibrary>), description: "Книги успешно получена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetBooksByIds([Required, FromQuery] string sourceIds)
    {
        var ids = QueryValuesToListConverter.Convert(sourceIds);
        if (ids == null)
        {
            _logger.LogWarning("Bad Request. List of source ids is empty.");

            return BadRequest();
        }
        
        try
        {
            var library = await _libraryService.GetBooksWithLibrariesAsync(ids);
            
            var dtoLibraries = library.ConvertAll(LibraryWithBookConverter.Convert);

            return Ok(dtoLibraries);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetBooksByIds));
            
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("{libraryUid:guid}/books/{bookUid:guid}/checkin")]
    [SwaggerOperation("Метод для возврата книги.", "Метод для возврата книги.")]
    [SwaggerResponse(statusCode: 200, type: typeof(CheckInBookResponse), description: "Книга успешно возвращена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CheckInBook([Required] [FromRoute] Guid libraryUid,
        [Required] [FromRoute] Guid bookUid,
        [Required] [FromBody] BookCondition bookCondition)
    {
        try
        {
            var item = await _libraryService.CheckInBookAsync(libraryUid, bookUid, BookConditionConvertor.Convert(bookCondition));
            
            var newDtoBook = LibraryBookConverter.Convert(item.NewItem);
            var oldDtoBook = LibraryBookConverter.Convert(item.OldItem);

            return Ok(new CheckInBookResponse(oldDtoBook, newDtoBook));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(CheckInBook));
            
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("{libraryUid:guid}/books/{bookUid:guid}/checkout")]
    [SwaggerOperation("Метод для аренды книги.", "Метод для аренды книги.")]
    [SwaggerResponse(statusCode: 200, type: typeof(LibraryBook), description: "Книга успешно арендована.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CheckOutBook([Required] [FromRoute] Guid libraryUid,
        [Required] [FromRoute] Guid bookUid)
    {
        try
        {
            var item = await _libraryService.CheckOutBookAsync(libraryUid, bookUid);
            
            var dtoBook = LibraryBookConverter.Convert(item);

            return Ok(dtoBook);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(CheckOutBook));
            
            return StatusCode(500, e.Message);
        }
    }
}