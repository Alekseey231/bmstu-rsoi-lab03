using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Базовая информация о книге
/// </summary>
[DataContract]
public class BookInfo
{
    /// <summary>
    /// UUID книги
    /// </summary>
    [Required]
    [DataMember(Name = "bookUid")]
    public Guid BookUid { get; set; }

    /// <summary>
    /// Название книги
    /// </summary>
    [DataMember(Name = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// Автор
    /// </summary>
    [DataMember(Name = "author")]
    public string? Author { get; set; }

    /// <summary>
    /// Жанр
    /// </summary>
    [DataMember(Name = "genre")]
    public string? Genre { get; set; }

    public BookInfo(Guid bookUid, string name, string? author, string? genre)
    {
        BookUid = bookUid;
        Name = name;
        Author = author;
        Genre = genre;
    }

    public BookInfo(Guid bookUid)
    {
        BookUid = bookUid;
    }
}