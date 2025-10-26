using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibraryService.Dto.Http.Models;

[DataContract]
public class BookWithLibrary
{
    [Required]
    [DataMember(Name = "libraryBook")]
    public LibraryBook LibraryBook { get; set; }
    
    [Required]
    [DataMember(Name = "library")]
    public Library Library { get; set; }

    public BookWithLibrary(LibraryBook libraryBook, Library library)
    {
        LibraryBook = libraryBook;
        Library = library;
    }
}