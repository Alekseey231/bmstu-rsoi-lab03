using ReservationService.Dto.Http.Converters.Enums;
using CoreReservation = ReservationService.Core.Models.Reservation;
using DtoReservation = ReservationService.Dto.Http.Models.Reservation;

namespace ReservationService.Dto.Http.Converters;

public static class ReservationConverter
{
    public static DtoReservation Convert(CoreReservation model)
    {
        return new DtoReservation(model.ReservationId,
            model.UserName,
            model.BookId,
            model.LibraryId,
            ReservationStatusConverter.Convert(model.Status),
            DateOnly.FromDateTime(model.StartDate),
            DateOnly.FromDateTime(model.TillDate));
    }
}