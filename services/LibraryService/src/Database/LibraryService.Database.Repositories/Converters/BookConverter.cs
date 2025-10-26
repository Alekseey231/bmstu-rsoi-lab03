using LibraryService.Database.Repositories.Converters.Enums;
using DbBook = LibraryService.Database.Models.Book;
using CoreBook = LibraryService.Core.Models.Book;

namespace LibraryService.Database.Repositories.Converters;

public static class BookConverter
{
    public static CoreBook Convert(DbBook model)
    {
        return new CoreBook(model.BookId,
            model.Name,
            model.Author,
            model.Genre,
            BookConditionConverter.Convert(model.BookCondition));
    }
}