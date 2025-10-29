using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GatewayService.Dto.Http;

/// <summary>
/// Информация о библиотеке
/// </summary>
[DataContract]
public class LibraryResponse
{
    /// <summary>
    /// UUID библиотеки
    /// </summary>
    [Required]
    [DataMember(Name = "libraryUid")]
    public Guid LibraryUid { get; set; }

    /// <summary>
    /// Название библиотеки
    /// </summary>
    [Required]
    [DataMember(Name = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// Адрес библиотеки
    /// </summary>
    [Required]
    [DataMember(Name = "address")]
    public string? Address { get; set; }

    /// <summary>
    /// Город, в котором находится библиотека
    /// </summary>
    [Required]
    [DataMember(Name = "city")]
    public string? City { get; set; }

    public LibraryResponse(Guid libraryUid, string name, string address, string city)
    {
        LibraryUid = libraryUid;
        Name = name;
        Address = address;
        City = city;
    }

    public LibraryResponse(Guid libraryUid)
    {
        LibraryUid = libraryUid;
    }
}
