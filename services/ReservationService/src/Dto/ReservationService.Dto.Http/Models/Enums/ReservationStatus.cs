using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ReservationService.Dto.Http.Models.Enums;

[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum ReservationStatus
{
    [EnumMember(Value = "RENTED")]
    Rented = 0,
    
    [EnumMember(Value = "RETURNED")]
    Returned = 1,
    
    [EnumMember(Value = "EXPIRED")]
    Expired = 2,
}