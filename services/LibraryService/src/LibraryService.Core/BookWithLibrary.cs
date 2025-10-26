using LibraryService.Core.Models;

namespace LibraryService.Core;

public class BookWithLibrary
{
    public InventoryItem Book { get; }
    public Library Library { get; }

    public BookWithLibrary(InventoryItem book, Library library)
    {
        Book = book;
        Library = library;
    }
}