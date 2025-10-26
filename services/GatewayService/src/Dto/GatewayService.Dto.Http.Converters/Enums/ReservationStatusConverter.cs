using DtoReservationStatus = GatewayService.Dto.Http.Enums.ReservationStatus;
using ReservationServiceReservationStatus = ReservationService.Dto.Http.Models.Enums.ReservationStatus;

namespace GatewayService.Dto.Http.Converters.Enums;

public static class ReservationStatusConverter
{
    public static ReservationServiceReservationStatus Convert(DtoReservationStatus model)
    {
        return model switch
        {
            DtoReservationStatus.Expired => ReservationServiceReservationStatus.Expired,
            DtoReservationStatus.Rented => ReservationServiceReservationStatus.Rented,
            DtoReservationStatus.Returned => ReservationServiceReservationStatus.Returned,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static DtoReservationStatus Convert(ReservationServiceReservationStatus model)
    {
        return model switch
        {
            ReservationServiceReservationStatus.Expired => DtoReservationStatus.Expired,
            ReservationServiceReservationStatus.Rented => DtoReservationStatus.Rented,
            ReservationServiceReservationStatus.Returned => DtoReservationStatus.Returned,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}