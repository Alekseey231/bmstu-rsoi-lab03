using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RatingService.Dto.Http;

public class UpdateRatingRequest
{
    [Required]
    [Range(1, 100)]
    [DataMember(Name = "stars")]
    public int Stars { get; set; }

    public UpdateRatingRequest(int stars)
    {
        Stars = stars;
    }
}