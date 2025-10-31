using GatewayService.Core.Models.Enums;

namespace GatewayService.Core.Models;

public class LibraryRequest
{
    public Guid LibraryId { get; set; }
    
    public Guid BookId { get; set; }
    
    public BookCondition BookCondition { get; set; }
    
    public LibraryRequest(Guid libraryId, Guid bookId, BookCondition bookCondition)
    {
        LibraryId = libraryId;
        BookId = bookId;
        BookCondition = bookCondition;
    }
}