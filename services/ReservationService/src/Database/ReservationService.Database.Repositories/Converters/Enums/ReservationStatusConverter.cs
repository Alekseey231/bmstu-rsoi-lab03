using DbReservationStatus = ReservationService.Database.Models.Enums.ReservationStatus;
using CoreReservationStatus = ReservationService.Core.Models.Enums.ReservationStatus;

namespace ReservationService.Database.Repositories.Converters.Enums;

public static class ReservationStatusConverter
{
    public static CoreReservationStatus Convert(DbReservationStatus model)
    {
        return model switch
        {
            DbReservationStatus.Expired => CoreReservationStatus.Expired,
            DbReservationStatus.Rented => CoreReservationStatus.Rented,
            DbReservationStatus.Returned => CoreReservationStatus.Returned,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static DbReservationStatus Convert(CoreReservationStatus model)
    {
        return model switch
        {
            CoreReservationStatus.Expired => DbReservationStatus.Expired,
            CoreReservationStatus.Rented => DbReservationStatus.Rented,
            CoreReservationStatus.Returned => DbReservationStatus.Returned,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}