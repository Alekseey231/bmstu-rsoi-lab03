using Refit;
using ReservationService.Dto.Http.Models;
using ReservationService.Dto.Http.Models.Enums;

namespace GatewayService.Clients;

public interface IReservationServiceClient
{
    [Get("/api/v1/reservations")]
    Task<List<Reservation>> GetReservationsAsync(
        [Query] string userName,
        [Query] ReservationStatus? status = null);

    [Post("/api/v1/reservations")]
    Task<Reservation> CreateReservationAsync([Body] Reservation request);

    [Patch("/api/v1/reservations/{reservationId}")]
    Task<Reservation> UpdateReservationAsync(
        Guid reservationId,
        [Body] UpdateReservationRequest request);
    
    [Get("/api/v1/reservations/{reservationId}")]
    Task<Reservation> GetReservationByIdAsync(Guid reservationId);
    
    [Delete("/api/v1/reservations/{reservationId}")]
    Task<Reservation> DeleteReservationAsync(Guid reservationId);
}