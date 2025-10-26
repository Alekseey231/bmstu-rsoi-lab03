using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Описание ошибки
/// </summary>
[DataContract]
public class ErrorDescription
{
    [DataMember(Name = "field")]
    public string Field { get; set; }

    [DataMember(Name = "error")]
    public string Error { get; set; }

    public ErrorDescription(string field, string error)
    {
        Field = field;
        Error = error;
    }
}