using Microsoft.Extensions.Logging;
using ReservationService.Core.Interfaces;
using ReservationService.Core.Models;
using ReservationService.Core.Models.Enums;

namespace ReservationService.Services.ReservationService;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(IReservationRepository reservationRepository, ILogger<ReservationService> logger)
    {
        _reservationRepository = reservationRepository;
        _logger = logger;
    }


    public async Task<Reservation> CreateReservationAsync(Guid reservationId,
        string userName,
        Guid bookId,
        Guid libraryId,
        ReservationStatus status,
        DateTime startDate,
        DateTime tillDate)
    {
        _logger.LogDebug("Creation reservation {ReservationId} for user {UserName}", reservationId, userName);
        
        var result = await _reservationRepository.CreateReservationAsync(reservationId,
            userName,
            bookId,
            libraryId,
            status,
            startDate,
            tillDate);
        
        _logger.LogInformation("Created reservation {ReservationId} for user {UserName}", reservationId, userName);
        
        return result;
    }

    public async Task<Reservation> UpdateReservationAsync(Guid reservationId, DateTime returnedDate)
    {
        _logger.LogDebug("Updating reservation {ReservationId}", reservationId);
        
        var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
        
        var status = returnedDate > reservation.TillDate ? ReservationStatus.Expired : ReservationStatus.Returned;
        
        var newReservation = await _reservationRepository.UpdateReservationAsync(reservationId, status);
        
        _logger.LogInformation("Reservation {ReservationId} updated", reservationId);
        
        return newReservation;
    }

    public async Task<List<Reservation>> GetReservationByUserNameAsync(string userName, ReservationStatus? status)
    {
        _logger.LogDebug("Getting reservations for user {UserName}", userName);
        
        var result = await _reservationRepository.GetReservationByUserNameAsync(userName, status);
        
        _logger.LogInformation("Retrieved reservations for user {UserName}", userName);
        
        return result;
    }

    public async Task<Reservation> GetReservationByIdAsync(Guid reservationId)
    {
        _logger.LogDebug("Getting reservation {ReservationId}", reservationId);
        
        var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
        
        _logger.LogInformation("Retrieved reservation {ReservationId}", reservationId);
        
        return reservation;
    }

    public async Task DeleteReservationAsync(Guid reservationId)
    {
        _logger.LogDebug("Deleting reservation {ReservationId}", reservationId);
        
        await _reservationRepository.DeleteReservationAsync(reservationId);
        
        _logger.LogInformation("Reservation {ReservationId} deleted", reservationId);
    }
}