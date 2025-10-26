using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Ответ с ошибкой валидации
/// </summary>
[DataContract]
public class ValidationErrorResponse
{
    /// <summary>
    /// Информация об ошибке
    /// </summary>
    [Required]
    [DataMember(Name = "message")]
    public string Message { get; set; }

    /// <summary>
    /// Массив полей с описанием ошибки
    /// </summary>
    [Required]
    [DataMember(Name = "errors")]
    public List<ErrorDescription> Errors { get; set; }

    public ValidationErrorResponse(string message, List<ErrorDescription> errors)
    {
        Message = message;
        Errors = errors;
    }
}