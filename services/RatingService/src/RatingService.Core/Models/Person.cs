namespace RatingService.Core.Models;

public class Rating
{
    public string UserName { get; set; }
    
    public int Stars { get; set; }

    public Rating(string userName, int stars)
    {
        UserName = userName;
        Stars = stars;
    }
}