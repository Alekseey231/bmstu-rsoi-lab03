namespace GatewayService.Server.Configurations;

public class HttpClientConfig
{
    public string LibraryServiceUrl { get; set; }
    public string RatingServiceUrl { get; set; }
    public string ReservationServiceUrl { get; set; }

    public HttpClientConfig(string libraryServiceUrl, string ratingServiceUrl, string reservationServiceUrl)
    {
        LibraryServiceUrl = libraryServiceUrl;
        RatingServiceUrl = ratingServiceUrl;
        ReservationServiceUrl = reservationServiceUrl;
    }
}