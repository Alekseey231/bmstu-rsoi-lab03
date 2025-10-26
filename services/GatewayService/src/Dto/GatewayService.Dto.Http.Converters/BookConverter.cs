using GatewayService.Dto.Http.Converters.Enums;
using LibraryService.Dto.Http.Models;

namespace GatewayService.Dto.Http.Converters;

public static class BookConverter
{
    public static LibraryBookResponse Convert(LibraryBook book)
    {
        return new LibraryBookResponse(book.BookUid,
            book.Name,
            book.Author,
            book.Genre,
            BookConditionConverter.Convert(book.Condition),
            book.AvailableCount);
    }

    public static BookInfo ConvertToBookInfo(LibraryBook book)
    {
        return new BookInfo(book.BookUid,
            book.Name,
            book.Author,
            book.Genre);
    }
}