using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LibraryService.Dto.Http.Models;

namespace LibraryService.Dto.Http;

[DataContract]
public class CheckInBookResponse
{
    [Required]
    [DataMember(Name = "oldBook")]
    public LibraryBook OldBook { get; set; }
    
    [Required]
    [DataMember(Name = "newBook")]
    public LibraryBook NewBook { get; set; }

    public CheckInBookResponse(LibraryBook oldBook, LibraryBook newBook)
    {
        OldBook = oldBook;
        NewBook = newBook;
    }
}