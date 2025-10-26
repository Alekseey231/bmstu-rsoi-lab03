using ReservationService.Database.Repositories.Converters.Enums;
using DbReservation = ReservationService.Database.Models.Reservation;
using CoreReservation = ReservationService.Core.Models.Reservation;

namespace ReservationService.Database.Repositories.Converters;

public static class ReservationConverter
{
    public static CoreReservation Convert(DbReservation model)
    {
        return new CoreReservation(model.ReservationId,
            model.UserName,
            model.BookId,
            model.LibraryId,
            ReservationStatusConverter.Convert(model.Status),
            model.StartDate,
            model.TillDate);
    }
}