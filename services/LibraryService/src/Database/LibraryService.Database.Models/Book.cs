using LibraryService.Database.Models.Enums;

namespace LibraryService.Database.Models;

public class Book
{
    #region Properties
    
    public int Id { get; set; }
    
    public Guid BookId { get; set; }
    
    public string Name { get; set; }
    
    public string? Author { get; set; }
    
    public string? Genre { get; set; }
    
    public BookCondition BookCondition { get; set; }
    
    #endregion
    
    #region Navigation Properties
    
    public ICollection<LibraryBook>? LibrariesBooks { get; set; }
    
    #endregion

    public Book(Guid bookId, string name, string? author, string? genre, BookCondition bookCondition)
    {
        BookId = bookId;
        Name = name;
        Author = author;
        Genre = genre;
        BookCondition = bookCondition;
    }
}