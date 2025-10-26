using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Запрос на взятие книги
/// </summary>
[DataContract]
public class TakeBookRequest
{
    /// <summary>
    /// UUID книги
    /// </summary>
    [Required]
    [DataMember(Name = "bookUid")]
    public Guid BookUid { get; set; }

    /// <summary>
    /// UUID библиотеки
    /// </summary>
    [Required]
    [DataMember(Name = "libraryUid")]
    public Guid LibraryUid { get; set; }

    /// <summary>
    /// Дата окончания бронирования
    /// </summary>
    [Required]
    [DataMember(Name = "tillDate")]
    public DateOnly TillDate { get; set; }

    public TakeBookRequest(Guid bookUid, Guid libraryUid, DateOnly tillDate)
    {
        BookUid = bookUid;
        LibraryUid = libraryUid;
        TillDate = tillDate;
    }
}