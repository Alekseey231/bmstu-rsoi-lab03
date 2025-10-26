using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Ответ с ошибкой
/// </summary>
[DataContract]
public class ErrorResponse
{
    /// <summary>
    /// Информация об ошибке
    /// </summary>
    [Required]
    [DataMember(Name = "message")]
    public string Message { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
    }
}