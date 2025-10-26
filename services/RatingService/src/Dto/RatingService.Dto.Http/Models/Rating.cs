using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RatingService.Dto.Http.Models;

[DataContract]
public class Rating
{
    [Required]
    [DataMember(Name = "userName")]
    public string UserName { get; set; }
    
    [Required]
    [DataMember(Name = "stars")]
    public int Stars { get; set; }

    public Rating(string userName, int stars)
    {
        UserName = userName;
        Stars = stars;
    }
}