using System.ComponentModel.DataAnnotations;
using System.Net;
using GatewayService.Clients;
using GatewayService.Core.Exceptions;
using GatewayService.Core.Interfaces;
using GatewayService.Core.Models;
using GatewayService.Core.Models.Enums;
using GatewayService.Dto.Http;
using GatewayService.Dto.Http.Converters;
using GatewayService.Dto.Http.Converters.Enums;
using GatewayService.Services.CircuitBreaker.Exceptions;
using LibraryService.Dto.Http.Models;
using Microsoft.AspNetCore.Mvc;
using RatingService.Dto.Http;
using RatingService.Dto.Http.Models;
using Refit;
using ReservationService.Dto.Http.Models;
using Swashbuckle.AspNetCore.Annotations;
using ErrorResponse = GatewayService.Dto.Http.ErrorResponse;
using ReservationServiceReservationStatus = ReservationService.Dto.Http.Models.Enums.ReservationStatus;
using ReturnBookRequest = GatewayService.Dto.Http.ReturnBookRequest;

namespace GatewayService.Server.Controllers;

[ApiController]
[Route("/api/v1/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationServiceClient _reservationServiceRequestClient;
    private readonly IRatingServiceClient _ratingServiceRequestClient;
    private readonly ILibraryServiceClient _libraryServiceRequestClient;
    private readonly ILibraryServiceRequestsQueue _libraryServiceRequestsQueue;
    private readonly IRatingServiceRequestsQueue _ratingServiceRequestsQueue;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(IReservationServiceClient reservationServiceRequestClient,
        ILibraryServiceRequestsQueue libraryServiceRequestsQueue,
        IRatingServiceRequestsQueue ratingServiceRequestsQueue,
        IRatingServiceClient ratingServiceRequestClient,
        ILibraryServiceClient libraryServiceRequestClient,
        ILogger<ReservationController> logger)
    {
        _libraryServiceRequestsQueue = libraryServiceRequestsQueue;
        _ratingServiceRequestsQueue = ratingServiceRequestsQueue;
        _ratingServiceRequestClient = ratingServiceRequestClient;
        _reservationServiceRequestClient = reservationServiceRequestClient;
        _libraryServiceRequestClient = libraryServiceRequestClient;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation("Получить информацию по всем взятым в прокат книгам пользователя", "Получить информацию по всем взятым в прокат книгам пользователя")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<BookReservationResponse>), description: "Информация по всем взятым в прокат книгам")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера")]
    public async Task<IActionResult> GetReservations([Required][FromHeader(Name = "X-User-Name")] string userName)
    {
        try
        {
            var userReservations = await _reservationServiceRequestClient.GetReservationsAsync(userName);
        
            if (userReservations.Count == 0)
                return Ok(new List<BookReservationResponse>());

            var bookIds = userReservations.Select(r => r.BookId).ToList();
            var booksWithLibraries = await TryGetBookWithLibraryByIds(bookIds);
            var bookLookup = booksWithLibraries.ToDictionary(b => b.LibraryBook.BookUid);
        
            var responses = userReservations
                .Select(reservation => MapToBookReservationResponse(reservation, bookLookup))
                .ToList();
        
            return Ok(responses);
        }
        catch (BrokenCircuitException e)
        {
            _logger.LogError(e, "Reservation service unavailable");
            
            return StatusCode(503, new ErrorResponse("Reservation service unavailable."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetReservations));
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }

    [HttpPost]
    [SwaggerOperation("Взять книгу в библиотеке", "Взять книгу в библиотеке")]
    [SwaggerResponse(statusCode: 200, type: typeof(TakeBookResponse), description: "Информация о бронировании")]
    [SwaggerResponse(statusCode: 400, type: typeof(ValidationErrorResponse), description: "Ошибка валидации данных")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера")]
    public async Task<IActionResult> TakeBook([Required][FromHeader(Name = "X-User-Name")] string userName,
        [Required][FromBody] TakeBookRequest request)
    {
        try
        {
            var bookWithLibrary = await _libraryServiceRequestClient.GetBookAsync(request.LibraryUid, request.BookUid);
            
            var userReservations = await _reservationServiceRequestClient.GetReservationsAsync(userName,
                    ReservationServiceReservationStatus.Rented);

            //TODO: костыль, вынести на отдельный уровень.
            Rating currentRating;
            try
            {
                currentRating = await _ratingServiceRequestClient.GetRatingAsync(userName);
            }
            catch (ApiException e) when (e.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                _logger.LogError(e, "Bonus Service unavailable");

                return StatusCode(503, new ErrorResponse("Bonus Service unavailable"));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Bonus Service unavailable");
            
                return StatusCode(503, new ErrorResponse("Bonus Service unavailable"));
            }


            if (userReservations.Count >= currentRating.Stars)
                throw new MaxBooksLimitExceededException($"Count took books limit exceeded for user {userName}. Current rating {currentRating.Stars}.");
            
            var newReservation = await _reservationServiceRequestClient.CreateReservationAsync(new Reservation(
                Guid.NewGuid(),
                userName,
                request.BookUid,
                request.LibraryUid,
                ReservationServiceReservationStatus.Rented,
                DateOnly.FromDateTime(DateTime.Now),
                request.TillDate));

            try
            {
                var book = await _libraryServiceRequestClient.CheckOutBookAsync(request.LibraryUid, request.BookUid);
            
                var dtoBook = BookConverter.ConvertToBookInfo(book);
                var dtoRating = RatingConverter.Convert(currentRating);
                var dtoLibrary = LibraryConverter.Convert(bookWithLibrary.Library);
            
                var result = new TakeBookResponse(newReservation.ReservationId,
                    ReservationStatusConverter.Convert(newReservation.Status),
                    newReservation.StartDate,
                    newReservation.TillDate,
                    dtoBook,
                    dtoLibrary,
                    dtoRating);
                
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Error while checking out book {BookId} in library {LibraryId} for user {UserId}",
                    request.BookUid, 
                    request.LibraryUid,
                    userName);
                
                await _reservationServiceRequestClient.DeleteReservationAsync(newReservation.ReservationId);
                
                return StatusCode(503, new ErrorResponse("Library service unavailable."));
            }
        }
        catch (MaxBooksLimitExceededException e)
        {
            _logger.LogWarning(e, "Count took books limit exceeded for user {UserName}.", userName);

            return StatusCode(403,
                new ErrorResponse("Превышен лимит количества одновременно разрешенных для аренды книг."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(TakeBook));
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }

    [HttpPost("{reservationUid:guid}/return")]
    [SwaggerOperation("Вернуть книгу", "Вернуть книгу")]
    [SwaggerResponse(statusCode: 204, description: "Книга успешно возвращена")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Бронирование не найдено")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера")]
    public async Task<IActionResult> ReturnBook(
        [Required][FromRoute] Guid reservationUid,
        [Required][FromHeader(Name = "X-User-Name")] string userName,
        [Required][FromBody] ReturnBookRequest request)
    {
        try
        {
            
            var penalty = 0;
            
            var closedReservation = await _reservationServiceRequestClient.UpdateReservationAsync(reservationUid,
                new UpdateReservationRequest(request.Date));
            
            if (closedReservation.Status == ReservationServiceReservationStatus.Expired)
                penalty += 10;

            try
            {
                var checkInBookResponse = await _libraryServiceRequestClient.CheckInBookAsync(closedReservation.LibraryId, 
                    closedReservation.BookId,
                    BookConditionConverter.Convert(request.Condition));
            
                if (checkInBookResponse.NewBook.Condition != checkInBookResponse.OldBook.Condition)
                    penalty += 10;
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Error while check in book {BookId} in library {LibraryId} for user {User}. Enqueue...",
                    closedReservation.BookId,
                    closedReservation.LibraryId,
                    userName);
                
                await _libraryServiceRequestsQueue.EnqueueAsync(new Core.Models.ReturnBookRequest(ReturnBookRequestState.LibraryRequestFailed,
                    penalty,
                    new RatingRequest(userName),
                    new LibraryRequest(closedReservation.LibraryId,
                        closedReservation.BookId,
                        BookConditionConverter.ConvertToCore(request.Condition))));
                
                return NoContent();
            }

            try
            {
                var rating = await _ratingServiceRequestClient.GetRatingAsync(userName);
                
                var newCountStars = penalty == 0 ? rating.Stars + 1 : rating.Stars - penalty;
                
                await _ratingServiceRequestClient.UpdateRatingAsync(userName, new UpdateRatingRequest(newCountStars));
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Error while update rating after check in {BookId} in library {LibraryId} for user {User}. Enqueue...",
                    closedReservation.BookId,
                    closedReservation.LibraryId,
                    userName);
                
                await _ratingServiceRequestsQueue.EnqueueAsync(new Core.Models.ReturnBookRequest(ReturnBookRequestState.RatingRequestFailed,
                    penalty,
                    new RatingRequest(userName)));
                
                return NoContent();
            }
            
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(ReturnBook));
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    private BookReservationResponse MapToBookReservationResponse(Reservation reservation, 
        Dictionary<Guid, BookWithLibrary> bookLookup)
    {
        BookInfo bookInfo;
        LibraryResponse libraryResponse;
    
        if (bookLookup.TryGetValue(reservation.BookId, out var bookWithLibrary))
        {
            bookInfo = BookConverter.ConvertToBookInfo(bookWithLibrary.LibraryBook);
            libraryResponse = LibraryConverter.Convert(bookWithLibrary.Library);
        }
        else
        {
            bookInfo = new BookInfo(reservation.BookId);
            libraryResponse = new LibraryResponse(reservation.LibraryId);
        }
    
        return new BookReservationResponse(
            reservation.ReservationId,
            ReservationStatusConverter.Convert(reservation.Status),
            reservation.StartDate,
            reservation.TillDate,
            bookInfo,
            libraryResponse);
    }

    private async Task<List<BookWithLibrary>> TryGetBookWithLibraryByIds(List<Guid> bookIds)
    {
        try
        {
            var ids = string.Join(",", bookIds);

            return await _libraryServiceRequestClient.GetBooksByIds(ids);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting books by ids");
            
            return new List<BookWithLibrary>();
        }
    }
}