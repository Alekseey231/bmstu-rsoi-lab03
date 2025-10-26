using LibraryService.Core.Models.Enums;

namespace LibraryService.Core.Models;

public class Book
{
    public Guid BookId { get; set; }
    
    public string Name { get; set; }
    
    public string? Author { get; set; }
    
    public string? Genre { get; set; }
    
    public BookCondition BookCondition { get; set; }

    public Book(Guid bookId, string name, string? author, string? genre, BookCondition bookCondition)
    {
        BookId = bookId;
        Name = name;
        Author = author;
        Genre = genre;
        BookCondition = bookCondition;
    }
}