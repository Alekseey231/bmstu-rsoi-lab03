using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace LibraryService.Dto.Http.Models.Enums;

[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum BookCondition
{
    [EnumMember(Value = "Excellent")]
    Excellent = 0,
    
    [EnumMember(Value = "Good")]
    Good = 1,
    
    [EnumMember(Value = "Bad")]
    Bad = 2
}