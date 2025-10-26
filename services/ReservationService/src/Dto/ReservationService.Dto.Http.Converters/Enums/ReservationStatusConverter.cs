using System.Diagnostics.CodeAnalysis;
using CoreReservationStatus = ReservationService.Core.Models.Enums.ReservationStatus;
using DtoReservationStatus = ReservationService.Dto.Http.Models.Enums.ReservationStatus;

namespace ReservationService.Dto.Http.Converters.Enums;

public static class ReservationStatusConverter
{
    public static DtoReservationStatus Convert(CoreReservationStatus model)
    {
        return model switch
        {
            CoreReservationStatus.Expired => DtoReservationStatus.Expired,
            CoreReservationStatus.Rented => DtoReservationStatus.Rented,
            CoreReservationStatus.Returned => DtoReservationStatus.Returned,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    [return: NotNullIfNotNull("model")]
    public static CoreReservationStatus? Convert(DtoReservationStatus? model)
    {
        if (model is null)
            return null;
        
        return model switch
        {
            DtoReservationStatus.Expired => CoreReservationStatus.Expired,
            DtoReservationStatus.Rented => CoreReservationStatus.Rented,
            DtoReservationStatus.Returned => CoreReservationStatus.Returned,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}