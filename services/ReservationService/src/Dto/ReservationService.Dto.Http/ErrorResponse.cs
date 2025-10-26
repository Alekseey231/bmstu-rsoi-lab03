using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ReservationService.Dto.Http;

[DataContract]
public class ErrorResponse
{
    /// <summary>
    /// Описание ошибки.
    /// </summary>
    [Required]
    [DataMember(Name = "message")]
    public string Message { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
    }
}