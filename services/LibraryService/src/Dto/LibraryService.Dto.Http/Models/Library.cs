using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraryService.Dto.Http.Models;

[DataContract]
public class Library
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
    public string Name { get; set; }

    /// <summary>
    /// Адрес библиотеки
    /// </summary>
    [Required]
    [DataMember(Name = "address")]
    public string Address { get; set; }

    /// <summary>
    /// Город, в котором находится библиотека
    /// </summary>
    [Required]
    [DataMember(Name = "city")]
    public string City { get; set; }

    public Library(Guid libraryUid, string name, string address, string city)
    {
        LibraryUid = libraryUid;
        Name = name;
        Address = address;
        City = city;
    }
}