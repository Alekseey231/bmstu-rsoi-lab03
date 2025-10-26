using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Ответ с рейтингом пользователя
/// </summary>
[DataContract]
public class UserRatingResponse
{
    /// <summary>
    /// Количество звезд у пользователя
    /// </summary>
    [Required]
    [Range(0, 100)]
    [DataMember(Name = "stars")]
    public int Stars { get; set; }

    public UserRatingResponse(int stars)
    {
        Stars = stars;
    }
}