using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using GatewayService.Dto.Http.Enums;

namespace GatewayService.Dto.Http;

/// <summary>
/// Запрос на возврат книги
/// </summary>
[DataContract]
public class ReturnBookRequest
{
    /// <summary>
    /// Состояние книги
    /// </summary>
    [Required]
    [DataMember(Name = "condition")]
    public BookCondition Condition { get; set; }

    /// <summary>
    /// Дата возврата
    /// </summary>
    [Required]
    [DataMember(Name = "date")]
    public DateOnly Date { get; set; }

    public ReturnBookRequest(BookCondition condition, DateOnly date)
    {
        Condition = condition;
        Date = date;
    }
}
