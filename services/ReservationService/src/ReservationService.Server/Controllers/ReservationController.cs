using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ReservationService.Core.Interfaces;
using ReservationService.Dto.Http;
using ReservationService.Dto.Http.Converters;
using ReservationService.Dto.Http.Converters.Enums;
using ReservationService.Dto.Http.Models;
using ReservationService.Dto.Http.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace ReservationService.Server.Controllers;

[ApiController]
[Route("/api/v1/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(IReservationService reservationService, 
        ILogger<ReservationController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }
    
    [HttpPost]
    [SwaggerOperation("Метод для резервирования книги.", "Метод для резервирования книги.")]
    [SwaggerResponse(statusCode: 201, type: typeof(Reservation), description: "Книга успешно зарезервирована.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CreateReservation([Required] [FromBody] Reservation reservation)
    {
        try
        {
            var createdReservation = await _reservationService.CreateReservationAsync(reservation.ReservationId,
                reservation.UserName,
                reservation.BookId,
                reservation.LibraryId,
                ReservationStatusConverter.Convert(reservation.Status).Value,
                reservation.StartDate.ToDateTime(TimeOnly.MinValue),
                reservation.TillDate.ToDateTime(TimeOnly.MinValue));

            var dtoReservation = ReservationConverter.Convert(createdReservation);

            return Created($"/api/v1/reservations/{createdReservation.ReservationId}", dtoReservation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(CreateReservation));
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    [HttpPatch("{reservationId:guid}")]
    [SwaggerOperation("Метод для резервирования книги.", "Метод для резервирования книги.")]
    [SwaggerResponse(statusCode: 200, type: typeof(Reservation), description: "Книга успешно зарезервирована.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> UpdateReservation([Required] [FromRoute] Guid reservationId,
        [Required] [FromBody] UpdateReservationRequest request)
    {
        try
        {
            var updatedReservation = await _reservationService.UpdateReservationAsync(reservationId,
                request.ReturnedDate.ToDateTime(TimeOnly.MinValue));

            var dtoReservation = ReservationConverter.Convert(updatedReservation);

            return Ok(dtoReservation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(UpdateReservation));
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    [HttpDelete("{reservationId:guid}")]
    [SwaggerOperation("Метод для удаления резервирования книги.", "Метод для удаления резервирования книги.")]
    [SwaggerResponse(statusCode: 200, description: "Резервация книги успешно снята.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> DeleteReservation([Required] [FromRoute] Guid reservationId)
    {
        try
        {
            await _reservationService.DeleteReservationAsync(reservationId);
            
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(UpdateReservation));
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    [HttpGet]
    [SwaggerOperation("Метод для получения резервирований книг.", "Метод для получения резервирований книг.")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<Reservation>), description: "Резервации успешно получены.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetReservations([Required][FromQuery] string userName,
        [FromQuery] ReservationStatus? status)
    {
        try
        {
            var reservations = await _reservationService.GetReservationByUserNameAsync(userName,
                ReservationStatusConverter.Convert(status));
            
            var dtoReservations = reservations.ConvertAll(ReservationConverter.Convert);
            
            return Ok(dtoReservations);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetReservations));
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    [HttpGet("{reservationId:guid}")]
    [SwaggerOperation("Метод для получения резервирований книги.", "Метод для получения резервирований книги.")]
    [SwaggerResponse(statusCode: 200, type: typeof(Reservation), description: "Резервации успешно получены.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetReservationById([Required][FromRoute] Guid reservationId)
    {
        try
        {
            var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
            
            var dtoReservation = ReservationConverter.Convert(reservation);
            
            return Ok(dtoReservation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetReservationById));
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}