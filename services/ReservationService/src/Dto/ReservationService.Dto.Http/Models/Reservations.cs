using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ReservationService.Dto.Http.Models;

[DataContract]
public class Reservations
{
    [Required]
    [DataMember(Name = "reservations")]
    public List<Reservation> Items { get; set; }

    public Reservations(List<Reservation> items)
    {
        Items = items;
    }
}