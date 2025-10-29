namespace RatingService.Database.Models;

public class Rating
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public int Stars { get; set; }

    public Rating(string userName, int stars)
    {
        UserName = userName;
        Stars = stars;
    }
}