using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LibraryService.Dto.Http.Models.Enums;

namespace LibraryService.Dto.Http.Models;

[DataContract]
public class LibraryBook
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
    [Required]
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Автор
    /// </summary>
    [Required]
    [DataMember(Name = "author")]
    public string? Author { get; set; }

    /// <summary>
    /// Жанр
    /// </summary>
    [Required]
    [DataMember(Name = "genre")]
    public string? Genre { get; set; }

    /// <summary>
    /// Состояние книги
    /// </summary>
    [Required]
    [DataMember(Name = "condition")]
    public BookCondition Condition { get; set; }

    /// <summary>
    /// Количество книг, доступных для аренды в библиотеке
    /// </summary>
    [Required]
    [DataMember(Name = "availableCount")]
    public int AvailableCount { get; set; }

    public LibraryBook(Guid bookUid,
        string name,
        string? author,
        string? genre,
        BookCondition condition,
        int availableCount)
    {
        BookUid = bookUid;
        Name = name;
        Author = author;
        Genre = genre;
        Condition = condition;
        AvailableCount = availableCount;
    }
}