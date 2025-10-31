using ReservationService.Core.Models;
using ReservationService.Core.Models.Enums;

namespace ReservationService.Core.Interfaces;

/// <summary>
/// Интерфейс репозитория для резервирования.
/// </summary>
public interface IReservationRepository
{
    public Task<Reservation> CreateReservationAsync(Guid reservationId,
        string userName,
        Guid bookId,
        Guid libraryId,
        ReservationStatus status,
        DateTime startDate,
        DateTime tillDate);
    
    public Task<Reservation> UpdateReservationAsync(Guid reservationId, ReservationStatus status);

    public Task<Reservation> GetReservationByIdAsync(Guid reservationId);
    
    public Task<List<Reservation>> GetReservationByUserNameAsync(string userName, ReservationStatus? status);
    
    public Task DeleteReservationAsync(Guid reservationId);
}