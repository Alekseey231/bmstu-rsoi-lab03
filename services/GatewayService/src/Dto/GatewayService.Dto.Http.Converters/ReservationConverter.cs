using GatewayService.Dto.Http.Converters.Enums;
using ReservationService.Dto.Http.Models;

namespace GatewayService.Dto.Http.Converters;

public static class ReservationConverter
{
    public static BookReservationResponse Convert(Reservation reservation, BookInfo bookInfo, LibraryResponse libraryResponse)
    {
        return new BookReservationResponse(reservation.ReservationId,
            ReservationStatusConverter.Convert(reservation.Status),
            reservation.StartDate,
            reservation.TillDate,
            bookInfo,
            libraryResponse);
    }
}