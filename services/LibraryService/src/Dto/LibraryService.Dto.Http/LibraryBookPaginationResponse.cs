using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LibraryService.Dto.Http.Models;

namespace LibraryService.Dto.Http;

[DataContract]
public class LibraryBookPaginationResponse
{
    /// <summary>
    /// Номер страницы
    /// </summary>
    [Required]
    [DataMember(Name = "page")]
    public int Page { get; set; }

    /// <summary>
    /// Количество элементов на странице
    /// </summary>
    [Required]
    [DataMember(Name = "pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Общее количество элементов
    /// </summary>
    [Required]
    [DataMember(Name = "totalElements")]
    public int TotalElements { get; set; }

    /// <summary>
    /// Список книг
    /// </summary>
    [Required]
    [DataMember(Name = "items")]
    public List<LibraryBook> Items { get; set; }

    public LibraryBookPaginationResponse(int page, int pageSize, int totalElements, List<LibraryBook> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalElements = totalElements;
        Items = items;
    }
}