using LibraryService.Dto.Http.Converters.Enums;
using DtoLibraryBook = LibraryService.Dto.Http.Models.LibraryBook;
using CoreLibraryBook = LibraryService.Core.Models.InventoryItem;

namespace LibraryService.Dto.Http.Converters;

public static class LibraryBookConverter
{
    public static DtoLibraryBook Convert(CoreLibraryBook model)
    {
        var book = model.Book;

        return new DtoLibraryBook(book.BookId,
            book.Name,
            book.Author,
            book.Genre,
            BookConditionConvertor.Convert(book.BookCondition),
            model.AvailableCount);
    }
}