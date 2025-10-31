namespace GatewayService.Core.Models;

public class RatingRequest
{
    public string UserName { get; set; }

    public RatingRequest(string userName)
    {
        UserName = userName;
    }
}