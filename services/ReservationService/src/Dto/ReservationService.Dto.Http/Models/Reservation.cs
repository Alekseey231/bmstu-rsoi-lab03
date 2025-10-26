using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ReservationService.Dto.Http.Models.Enums;

namespace ReservationService.Dto.Http.Models;

[DataContract]
public class Reservation
{
    [Required]
    [DataMember(Name = "reservationId")]
    public Guid ReservationId { get; set; }
    
    [Required]
    [DataMember(Name = "userName")]
    public string UserName { get; set; }
    
    [Required]
    [DataMember(Name = "bookId")]
    public Guid BookId { get; set; }
    
    [Required]
    [DataMember(Name = "libraryId")]
    public Guid LibraryId { get; set; }
    
    [Required]
    [DataMember(Name = "status")]
    public ReservationStatus Status { get; set; }
    
    [Required]
    [DataMember(Name = "startDate")]
    public DateOnly StartDate { get; set; }
    
    [Required]
    [DataMember(Name = "tillDate")]
    public DateOnly TillDate { get; set; }

    public Reservation(Guid reservationId,
        string userName,
        Guid bookId, 
        Guid libraryId, 
        ReservationStatus status,
        DateOnly startDate, 
        DateOnly tillDate)
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