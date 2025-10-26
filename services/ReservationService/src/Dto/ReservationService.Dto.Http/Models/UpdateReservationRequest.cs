using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ReservationService.Dto.Http.Models;

[DataContract]
public class UpdateReservationRequest
{
    [Required]
    [DataMember(Name = "returnedDate")]
    public DateOnly ReturnedDate { get; set; }

    public UpdateReservationRequest(DateOnly returnedDate)
    {
        ReturnedDate = returnedDate;
    }
}