namespace LibraryService.Core.Models;

public class InventoryItem
{
    public Book Book { get; }
    public int AvailableCount { get; }

    public InventoryItem(Book book, int availableCount)
    {
        Book = book;
        AvailableCount = availableCount;
    }
}