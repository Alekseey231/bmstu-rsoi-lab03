using System.ComponentModel.DataAnnotations;
using GatewayService.Core.Exceptions;
using GatewayService.Dto.Http;
using GatewayService.Dto.Http.Converters;
using GatewayService.Dto.Http.Converters.Enums;
using GatewayService.Server.Clients;
using Microsoft.AspNetCore.Mvc;
using RatingService.Dto.Http;
using ReservationService.Dto.Http.Models;
using Swashbuckle.AspNetCore.Annotations;
using ErrorResponse = GatewayService.Dto.Http.ErrorResponse;
using ReservationServiceReservationStatus = ReservationService.Dto.Http.Models.Enums.ReservationStatus;

namespace GatewayService.Server.Controllers;

[ApiController]
[Route("/api/v1/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationServiceClient _reservationServiceRequestClient;
    private readonly IRatingServiceClient _ratingServiceRequestClient;
    private readonly ILibraryServiceClient _libraryServiceRequestClient;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(IReservationServiceClient reservationServiceRequestClient,
        IRatingServiceClient ratingServiceRequestClient,
        ILibraryServiceClient libraryServiceRequestClient,
        ILogger<ReservationController> logger)
    {
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
            var ids = string.Join(",", bookIds);

            var result = await _libraryServiceRequestClient.GetBooksByIds(ids);

            var responses = new List<BookReservationResponse>();
            foreach (var reservation in userReservations)
            {
                var book = result.First(b => b.LibraryBook.BookUid == reservation.BookId);
                
                var response = new BookReservationResponse(reservation.ReservationId,
                    ReservationStatusConverter.Convert(reservation.Status),
                    reservation.StartDate,
                    reservation.TillDate,
                    BookConverter.ConvertToBookInfo(book.LibraryBook),
                    LibraryConverter.Convert(book.Library));
                
                responses.Add(response);
            }

            return Ok(responses);
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
            
            var currentRating = await _ratingServiceRequestClient.GetRatingAsync(userName);

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
            var closedReservation = await _reservationServiceRequestClient.UpdateReservationAsync(reservationUid,
                new UpdateReservationRequest(request.Date));
            
            var checkInBookResponse = await _libraryServiceRequestClient.CheckInBookAsync(closedReservation.LibraryId, 
                closedReservation.BookId,
                BookConditionConverter.Convert(request.Condition));

            var penalty = 0;
            
            if (checkInBookResponse.NewBook.Condition != checkInBookResponse.OldBook.Condition)
                penalty += 10;
            
            if (closedReservation.Status == ReservationServiceReservationStatus.Expired)
                penalty += 10;
            
            var rating = await _ratingServiceRequestClient.GetRatingAsync(userName);
            
            var newCountStars = penalty == 0 ? rating.Stars + 1 : rating.Stars - penalty;
            
            await _ratingServiceRequestClient.UpdateRatingAsync(userName, new UpdateRatingRequest(newCountStars));
            
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(ReturnBook));
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}