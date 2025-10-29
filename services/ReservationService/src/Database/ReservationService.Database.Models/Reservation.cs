using ReservationService.Database.Models.Enums;

namespace ReservationService.Database.Models;

public class Reservation
{
    public int Id { get; set; }
    
    public Guid ReservationId { get; set; }
    
    public string UserName { get; set; }
    
    public Guid BookId { get; set; }
    
    public Guid LibraryId { get; set; }
    
    public ReservationStatus Status { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime TillDate { get; set; }

    public Reservation(Guid reservationId,
        string userName,
        Guid bookId, 
        Guid libraryId, 
        ReservationStatus status,
        DateTime startDate, 
        DateTime tillDate)
    {
        ReservationId = reservationId;
        UserName = userName;
        BookId = bookId;
        LibraryId = libraryId;
        Status = status;
        StartDate = startDate;
        TillDate = tillDate;
    }
}