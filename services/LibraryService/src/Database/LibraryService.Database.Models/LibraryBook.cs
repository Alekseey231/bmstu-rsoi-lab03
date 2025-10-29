namespace LibraryService.Database.Models;

public class LibraryBook
{
    #region Properties

    public int BookId { get; set; }
    
    public int LibraryId { get; set; }
    
    public int AvailableCount { get; set; }

    #endregion
    
    #region Navigation Properties
    
    public Book? Book { get; set; }
    
    public Library? Library { get; set; }
    
    #endregion

    public LibraryBook(int bookId, int libraryId, int availableCount)
    {
        BookId = bookId;
        LibraryId = libraryId;
        AvailableCount = availableCount;
    }
}