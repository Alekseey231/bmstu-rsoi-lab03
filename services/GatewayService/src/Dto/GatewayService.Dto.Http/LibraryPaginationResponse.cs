using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Ответ со списком библиотек с пагинацией
/// </summary>
[DataContract]
public class LibraryPaginationResponse
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
    /// Список библиотек
    /// </summary>
    [Required]
    [DataMember(Name = "items")]
    public List<LibraryResponse> Items { get; set; }

    public LibraryPaginationResponse(int page, int pageSize, int totalElements, List<LibraryResponse> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalElements = totalElements;
        Items = items;
    }
}