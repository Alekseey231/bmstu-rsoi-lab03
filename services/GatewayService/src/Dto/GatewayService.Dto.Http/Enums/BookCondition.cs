using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GatewayService.Dto.Http.Enums;

/// <summary>
/// Перечисление состояний книги
/// </summary>
[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum BookCondition
{
    [EnumMember(Value = "EXCELLENT")]
    Excellent = 0,
    
    [EnumMember(Value = "GOOD")]
    Good = 1,
    
    [EnumMember(Value = "BAD")]
    Bad = 2
}