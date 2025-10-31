using GatewayService.Core.Models.Enums;

namespace GatewayService.Core.Models;

public class ReturnBookRequest
{
    public LibraryRequest? LibraryRequest { get; set; }
    
    public RatingRequest RatingRequest { get; set; }
    
    public ReturnBookRequestState ReturnBookRequestState { get; set; }
    
    public int Penalty { get; set; }

    public ReturnBookRequest(ReturnBookRequestState returnBookRequestState,
        int penalty,
        RatingRequest ratingRequest, 
        LibraryRequest? libraryRequest = null)
    {
        Penalty = penalty;
        LibraryRequest = libraryRequest;
        RatingRequest = ratingRequest;
        ReturnBookRequestState = returnBookRequestState;
    }
}