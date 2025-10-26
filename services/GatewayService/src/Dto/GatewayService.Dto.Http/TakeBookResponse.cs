using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using GatewayService.Dto.Http.Enums;

namespace GatewayService.Dto.Http;

/// <summary>
/// Ответ на взятие книги
/// </summary>
[DataContract]
public class TakeBookResponse
{
    /// <summary>
    /// UUID бронирования
    /// </summary>
    [Required]
    [DataMember(Name = "reservationUid")]
    public Guid ReservationUid { get; set; }

    /// <summary>
    /// Статус бронирования книги
    /// </summary>
    [Required]
    [DataMember(Name = "status")]
    public ReservationStatus Status { get; set; }

    /// <summary>
    /// Дата начала бронирования
    /// </summary>
    [Required]
    [DataMember(Name = "startDate")]
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Дата окончания бронирования
    /// </summary>
    [Required]
    [DataMember(Name = "tillDate")]
    public DateOnly TillDate { get; set; }

    /// <summary>
    /// Информация о книге
    /// </summary>
    [Required]
    [DataMember(Name = "book")]
    public BookInfo Book { get; set; }

    /// <summary>
    /// Информация о библиотеке
    /// </summary>
    [Required]
    [DataMember(Name = "library")]
    public LibraryResponse Library { get; set; }

    /// <summary>
    /// Рейтинг пользователя
    /// </summary>
    [Required]
    [DataMember(Name = "rating")]
    public UserRatingResponse Rating { get; set; }

    public TakeBookResponse(Guid reservationUid, ReservationStatus status, DateOnly startDate, DateOnly tillDate, BookInfo book, LibraryResponse library, UserRatingResponse rating)
    {
        ReservationUid = reservationUid;
        Status = status;
        StartDate = startDate;
        TillDate = tillDate;
        Book = book;
        Library = library;
        Rating = rating;
    }
}